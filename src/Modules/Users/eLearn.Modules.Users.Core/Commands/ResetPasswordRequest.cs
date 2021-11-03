using MediatR;

namespace eLearn.Modules.Users.Core.Commands
{
    internal class ResetPasswordRequest : IRequest
    {
        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public string Token { get; set; } = string.Empty;
    }
}