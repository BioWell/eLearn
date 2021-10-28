using System.ComponentModel.DataAnnotations;
using MediatR;

namespace eLearn.Modules.Users.Core.Commands
{
    internal class SignUp : IRequest<int>
    {
        [Required] [EmailAddress] public string Email { get; set; } = string.Empty;

        [Required] public string Password { get; set; } = string.Empty;

        public string? Role { get; set; }
    }
}