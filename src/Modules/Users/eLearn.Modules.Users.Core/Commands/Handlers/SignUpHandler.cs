using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;

namespace eLearn.Modules.Users.Core.Commands.Handlers
{
    internal sealed class SignUpHandler: IRequestHandler<SignUp, int>
    {
        private readonly ILogger<SignUpHandler> _logger;

        public SignUpHandler(ILogger<SignUpHandler> logger)
        {
            _logger = logger;
        }

        public async Task<int> Handle(SignUp request, CancellationToken cancellationToken= default)
        {
            // _logger.LogInformation($"User with Email: '{SignUp.Email}' has signed up.");
            await Task.CompletedTask;
            return 1;
        }
    }
}