using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eLearn.Modules.Users.Core.Commands;
using eLearn.Modules.Users.Core.Entities;
using eLearn.Modules.Users.Core.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Shared.Infrastructure.Constants;
using Shared.Infrastructure.Services.Email;
using Shared.Infrastructure.Wrapper;

namespace eLearn.Modules.Users.Core.Services
{
    internal class IdentityService : IIdentityService
    {
        private readonly UserManager<AppUser> _userManager;
        // private readonly IJobService _jobService;
        private readonly IMailService _mailService;
        private readonly MailSettings _mailSettings;
        private readonly IStringLocalizer<IdentityService> _localizer;
        private readonly RegistrationOptions _registrationOptions;

        public IdentityService(
            UserManager<AppUser> userManager,
            // IJobService jobService,
            IMailService mailService,
            MailSettings mailSettings,
            IStringLocalizer<IdentityService> localizer, 
            RegistrationOptions registrationOptions)
        {
            _userManager = userManager;
            // _jobService = jobService;
            _mailService = mailService;
            _mailSettings = mailSettings;
            _localizer = localizer;
            _registrationOptions = registrationOptions;
        }

        public async Task<IResult> RegisterAsync(RegisterRequest request, string origin)
        {
            if (!_registrationOptions.Enabled)
            {
                throw new IdentityException(string.Format(_localizer["System SignUp Disabled."]));
            }

            var email = request.Email.ToLowerInvariant();
            var provider = email.Split("@").Last();
            if (_registrationOptions.InvalidEmailProviders?.Any(x => provider.Contains(x)) is true)
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
                FullName = request.FullName,
                PhoneNumber = request.PhoneNumber,
                IsDeleted = false
            };
            
            if (request.EmailConfirmed) user.EmailConfirmed = true;
            if (request.PhoneNumberConfirmed) user.PhoneNumberConfirmed = true;
            
            if (!string.IsNullOrWhiteSpace(request.PhoneNumber))
            {
                var userWithSamePhoneNumber = await _userManager.Users.FirstOrDefaultAsync(x => x.PhoneNumber == request.PhoneNumber);
                if (userWithSamePhoneNumber != null)
                {
                    throw new IdentityException(string.Format(_localizer["Phone number {0} is already registered."], request.PhoneNumber));
                }
            }
            
            // user.AddDomainEvent(new UserRegisteredEvent(user));
            var result = await _userManager.CreateAsync(user, request.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, RoleConstants.Guest);
                if (!_mailSettings.EnableVerification)// && !_smsSettings.EnableVerification)
                {
                    return await Result<string>.SuccessAsync(user.Id.ToString(), message: string.Format(_localizer["User {0} Registered."], user.UserName));
                }
                // TO DO
                var messages = new List<string> { string.Format(_localizer["User {0} Registered."], user.UserName) };
                
                return await Result<string>.SuccessAsync(user.Id.ToString(), messages: messages);
            }
            else
            {
                throw new IdentityException(_localizer["Validation Errors Occurred."], result.Errors.Select(a => _localizer[a.Description].ToString()).ToList());
            }
        }

        public Task<IResult<string>> ConfirmEmailAsync(string userId, string code)
        {
            throw new System.NotImplementedException();
        }

        public Task<IResult<string>> ConfirmPhoneNumberAsync(string userId, string code)
        {
            throw new System.NotImplementedException();
        }

        public Task<IResult> ForgotPasswordAsync(ForgotPasswordRequest request, string origin)
        {
            throw new System.NotImplementedException();
        }

        public Task<IResult> ResetPasswordAsync(ResetPasswordRequest request)
        {
            throw new System.NotImplementedException();
        }
    }
}