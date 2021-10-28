using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using eLearn.Modules.Users.Core.Entities;
using eLearn.Modules.Users.Core.Exceptions;
using eLearn.Modules.Users.Core.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace eLearn.Modules.Users.Core.Commands.Handlers
{
    internal sealed class SignUpHandler: IRequestHandler<SignUp>
    {
        private readonly ILogger<SignUpHandler> _logger;
        private readonly RegistrationOptions _registrationOptions;
        private readonly IUserRepository _userRepository;
        public SignUpHandler(ILogger<SignUpHandler> logger, RegistrationOptions registrationOptions, IUserRepository userRepository)
        {
            _logger = logger;
            _registrationOptions = registrationOptions;
            _userRepository = userRepository;
        }

        public async Task<Unit> Handle(SignUp command, CancellationToken cancellationToken= default)
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
            
            var user = await _userRepository.GetAsync(email);
            if (user is not null)
            {
                throw new EmailInUseException();
            }
            
            user = new ELearnUser
            {
                Id = Guid.NewGuid().ToString(),
                UserName = command.Email,
                Email = command.Email,
            };

            await _userRepository.AddAsync(user, command.Password);
            return Unit.Value;
        }
    }
}