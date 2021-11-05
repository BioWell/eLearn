using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using eLearn.Modules.Users.Core.Commands;
using eLearn.Modules.Users.Core.Dto.Identity.Tokens;
using eLearn.Modules.Users.Core.Entities;
using eLearn.Modules.Users.Core.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Shared.Infrastructure.Auth;
using Shared.Infrastructure.Hangfire;
using Shared.Infrastructure.Services.Email;
using Shared.Infrastructure.Utilities;
using Shared.Infrastructure.Wrapper;

namespace eLearn.Modules.Users.Core.Services
{
    internal class IdentityService : IIdentityService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IJobService _jobService;
        private readonly IMailService _mailService;
        private readonly MailSettings _mailSettings;
        private readonly AuthSettings _authSettings;
        private readonly IStringLocalizer<IdentityService> _localizer;
        private readonly RegistrationSettings _registrationSettings;
        private readonly ITokenService _tokenService;

        public IdentityService(
            UserManager<AppUser> userManager,
            IJobService jobService,
            IMailService mailService,
            MailSettings mailSettings,
            IStringLocalizer<IdentityService> localizer,
            RegistrationSettings registrationSettings,
            ITokenService tokenService,
            AuthSettings authSettings, 
            RoleManager<AppRole> roleManager)
        {
            _userManager = userManager;
            _jobService = jobService;
            _mailService = mailService;
            _mailSettings = mailSettings;
            _localizer = localizer;
            _registrationSettings = registrationSettings;
            _tokenService = tokenService;
            _authSettings = authSettings;
            _roleManager = roleManager;
        }

        public async Task<IResult> RegisterAsync(RegisterRequest request, string origin)
        {
            if (string.IsNullOrEmpty(origin))
            {
                throw new IdentityException(string.Format(_localizer["origin is empty."]));
            }

            if (!_registrationSettings.Enabled)
            {
                throw new IdentityException(string.Format(_localizer["System SignUp Disabled."]));
            }

            var email = request.Email.ToLowerInvariant();
            var provider = email.Split("@").Last();
            if (_registrationSettings.InvalidEmailProviders?.Any(x => provider.Contains(x)) is true)
            {
                throw new IdentityException(string.Format(_localizer["Email:{0} is invalid."], request.Email));
            }

            var userWithSameUserName = await _userManager.FindByNameAsync(request.Email);
            if (userWithSameUserName != null)
            {
                throw new IdentityException(string.Format(_localizer["Username {0} is already taken."], request.Email));
            }

            var user = new AppUser
            {
                UserName = request.Email,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                PhoneNumber = request.PhoneNumber,
                IsActive = true
            };

            if (request.EmailConfirmed) user.EmailConfirmed = true;
            if (request.PhoneNumberConfirmed) user.PhoneNumberConfirmed = true;

            if (!string.IsNullOrWhiteSpace(request.PhoneNumber))
            {
                var userWithSamePhoneNumber =
                    await _userManager.Users.FirstOrDefaultAsync(x => x.PhoneNumber == request.PhoneNumber);
                if (userWithSamePhoneNumber != null)
                {
                    throw new IdentityException(string.Format(_localizer["Phone number {0} is already registered."],
                        request.PhoneNumber));
                }
            }

            // user.AddDomainEvent(new UserRegisteredEvent(user));
            var result = await _userManager.CreateAsync(user, request.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, RoleConstants.SuperAdmin); // TO DO 
                
                var superAdminRoleInDb = await _roleManager.FindByNameAsync(RoleConstants.SuperAdmin);
                
                foreach (string permission in typeof(Shared.Infrastructure.Auth.Permissions).GetNestedClassesStaticStringValues())
                {
                    await _roleManager.AddPermissionClaimAsync(superAdminRoleInDb, permission);
                }
                
                if (!_mailSettings.EnableVerification)
                {
                    return await Result<string>.SuccessAsync(user.Id.ToString(),
                        message: string.Format(_localizer["User {0} Registered."], user.UserName));
                }

                var messages = new List<string> {string.Format(_localizer["User {0} Registered."], user.UserName)};

                if (_mailSettings.EnableVerification)
                {
                    // send verification email
                    string emailVerificationUri = await GetEmailVerificationUriAsync(user, origin);
                    var mailRequest = new MailRequest
                    {
                        From = _mailSettings.From,
                        To = user.Email,
                        Body = string.Format(
                            _localizer[
                                "hello, customer please confirm your Biowell account by <a href='{0}'>clicking here</a>."],
                            emailVerificationUri),
                        Subject = _localizer["Confirm Registration"]
                    };
                    _jobService.Enqueue(() => _mailService.SendAsync(mailRequest));

                    System.IO.File.WriteAllText("confirmationLink.txt", emailVerificationUri);

                    messages.Add(_localizer["Please check your Mailbox to verify!"]);
                }

                return await Result<string>.SuccessAsync(user.Id.ToString(), messages: messages);
            }
            else
            {
                throw new IdentityException(_localizer["Validation Errors Occurred."],
                    result.Errors.Select(a => _localizer[a.Description].ToString()).ToList());
            }
        }

        public async Task<IResult> LoginAsync(LoginRequest request, string? ipAddress)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                throw new IdentityException(_localizer["User Not Found."], statusCode: HttpStatusCode.Unauthorized);
            }

            if (await _userManager.IsLockedOutAsync(user))
            {
                throw new IdentityException(_localizer["User Is Locked. Please try it later."],
                    statusCode: HttpStatusCode.Forbidden);
            }

            if (!user.IsActive)
            {
                throw new IdentityException(_localizer["User Not Active. Please contact the administrator."],
                    statusCode: HttpStatusCode.Unauthorized);
            }

            if (_mailSettings.EnableVerification && !await _userManager.IsEmailConfirmedAsync(user))
            {
                throw new IdentityException(_localizer["E-Mail not confirmed."],
                    statusCode: HttpStatusCode.Unauthorized);
            }

            bool passwordValid = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!passwordValid)
            {
                await _userManager.AccessFailedAsync(user);
                if (await _userManager.IsLockedOutAsync(user))
                {
                    // email user, notifying them of lockout
                }

                throw new IdentityException(_localizer["Invalid Credentials."],
                    statusCode: HttpStatusCode.Unauthorized);
            }

            await _userManager.ResetAccessFailedCountAsync(user);

            if (await _userManager.GetTwoFactorEnabledAsync(user))
            {
            }

            user.RefreshToken = _tokenService.GenerateRefreshToken();
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_authSettings.RefreshTokenExpirationInDays);
            await _userManager.UpdateAsync(user);
            string token = await _tokenService.GenerateJwtAsync(user, ipAddress);
            var response = new TokenResponse(token, user.RefreshToken, user.RefreshTokenExpiryTime);
            // await _eventLog.LogCustomEventAsync(new() { Description = $"Generated Tokens for {user.Email}.", Email = user.Email });
            return await Result<TokenResponse>.SuccessAsync(response);
        }

        public async Task<IResult<string>> ConfirmEmailAsync(string userId, string code)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new IdentityException(_localizer["An error occurred while confirming E-Mail."]);
            }

            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
            {
                return await Result<string>.SuccessAsync(user.Id.ToString(),
                    string.Format(
                        _localizer[
                            "Account Confirmed for E-Mail {0}. You can now use the /api/identity/token endpoint to generate JWT."],
                        user.Email));
            }
            else
            {
                throw new IdentityException(string.Format(_localizer["An error occurred while confirming {0}"],
                    user.Email));
            }
        }

        public async Task<IResult> ForgotPasswordAsync(ForgotPasswordRequest request, string origin)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
            {
                // Don't reveal that the user does not exist or is not confirmed
                throw new IdentityException(_localizer["An Error has occurred!"]);
            }

            string code = await _userManager.GeneratePasswordResetTokenAsync(user);
            System.IO.File.WriteAllText("resetLink.txt", code);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            string route = "api/identity/reset-password";
            var endpointUri = new Uri(string.Concat($"{origin}/", route));
            string passwordResetUrl = QueryHelpers.AddQueryString(endpointUri.ToString(), "Token", code);
            var mailRequest = new MailRequest
            {
                Body = string.Format(_localizer["Please reset your password by <a href='{0}>clicking here</a>."],
                    HtmlEncoder.Default.Encode(passwordResetUrl)),
                Subject = _localizer["Reset Password"],
                To = request.Email
            };
            _jobService.Enqueue(() => _mailService.SendAsync(mailRequest));

            return await Result.SuccessAsync(_localizer["Password Reset Mail has been sent to your authorized Email."]);
        }

        public async Task<IResult> ResetPasswordAsync(ResetPasswordRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                throw new IdentityException(_localizer["An Error has occurred!"]);
            }

            var result = await _userManager.ResetPasswordAsync(user, request.Token, request.Password);
            if (result.Succeeded)
            {
                return await Result.SuccessAsync(_localizer["Password Reset Successful!"]);
            }
            else
            {
                throw new IdentityException(_localizer["An Error has occurred!"]);
            }
        }

        public Task<IResult> ChangePasswordAsync(ChangePasswordRequest request)
        {
            throw new NotImplementedException();
            // var user = await GetCurrentUserAsync();
            // if (user != null)
            // {
            //     var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
            //     if (result.Succeeded)
            //     {
            //         await _signInManager.SignInAsync(user, isPersistent: false);
            //         _logger.LogInformation(3, "User changed their password successfully.");
            //         return RedirectToAction(nameof(Index), new { Message = ManageMessageId.ChangePasswordSuccess });
            //     }
            //     AddErrors(result);
            //     return View(model);
            // }
            // return RedirectToAction(nameof(Index), new { Message = ManageMessageId.Error });
        }

        private async Task<string> GetEmailVerificationUriAsync(AppUser user, string origin)
        {
            string code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            string route = "api/users/identity/confirm-email/";
            var endpointUri = new Uri(string.Concat($"{origin}/", route));
            string verificationUri = QueryHelpers.AddQueryString(endpointUri.ToString(), "userId", user.Id.ToString());
            verificationUri = QueryHelpers.AddQueryString(verificationUri, "code", code);
            return verificationUri;
        }
    }
}