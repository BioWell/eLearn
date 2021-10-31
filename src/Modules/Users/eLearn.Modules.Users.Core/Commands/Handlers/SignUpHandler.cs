using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using eLearn.Modules.Users.Core.Entities;
using eLearn.Modules.Users.Core.Exceptions;
using eLearn.Modules.Users.Core.Repositories;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Shared.Infrastructure.Constants;
using Shared.Infrastructure.Services.Email;

namespace eLearn.Modules.Users.Core.Commands.Handlers
{
    internal sealed class SignUpHandler : IRequestHandler<SignUp>
    {
        private readonly ILogger<SignUpHandler> _logger;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMailService _mailService;
        private readonly MailSettings _mailSettings;
        private readonly RegistrationOptions _registrationOptions;

        public SignUpHandler(ILogger<SignUpHandler> logger,
            RegistrationOptions registrationOptions,
            IUserRepository userRepository,
            UserManager<AppUser> userManager,
            IMailService mailService,
            IOptions<MailSettings> mailSettings)
        {
            _logger = logger;
            _registrationOptions = registrationOptions;
            _userManager = userManager;
            _mailService = mailService;
            _mailSettings = mailSettings.Value;
        }

        public async Task<Unit> Handle(SignUp command, CancellationToken cancellationToken = default)
        {
            if (!_registrationOptions.Enabled)
            {
                throw new SignUpDisabledException();
            }

            var email = command.Email.ToLowerInvariant();
            var provider = email.Split("@").Last();
            if (_registrationOptions.InvalidEmailProviders?.Any(x => provider.Contains(x)) is true)
            {
                throw new InvalidEmailException(email);
            }

            var user = await _userManager.FindByNameAsync(command.Email);
            if (user is not null)
            {
                throw new EmailInUseException();
            }

            user = new AppUser
            {
                UserName = command.Email,
                Email = command.Email,
                FullName = command.FullName,
                PhoneNumber = command.PhoneNumber,
                IsDeleted = false
            };

            if (command.EmailConfirmed) user.EmailConfirmed = true;
            if (command.PhoneNumberConfirmed) user.PhoneNumberConfirmed = true;

            if (!string.IsNullOrWhiteSpace(command.PhoneNumber))
            {
                var userWithSamePhoneNumber =
                    await _userManager.Users.FirstOrDefaultAsync(x => x.PhoneNumber == command.PhoneNumber,
                        cancellationToken: cancellationToken);
                if (userWithSamePhoneNumber != null)
                {
                    throw new PhoneInUseException();
                }
            }

            var result = await _userManager.CreateAsync(user, command.Password);
            if (result.Succeeded)
            {
                try
                {
                    await _userManager.AddToRoleAsync(user, RoleConstants.Guest);
                }
                catch
                {
                }

                if (!_mailSettings.EnableVerification) //&& !_smsSettings.EnableVerification)
                {
                    return Unit.Value;
                }
                
                if (_mailSettings.EnableVerification)
                {
                    // send verification email

                }
            }
            else
            {
            }

            return Unit.Value;
        }
        
        private async Task<string> GetEmailVerificationUriAsync(AppUser user, string origin)
        {
            string code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            string route = "api/identity/confirm-email/";
            var endpointUri = new Uri(string.Concat($"{origin}/", route));
            string verificationUri = QueryHelpers.AddQueryString(endpointUri.ToString(), "userId", user.Id.ToString());
            verificationUri = QueryHelpers.AddQueryString(verificationUri, "code", code);
            return verificationUri;
        }
    }
}