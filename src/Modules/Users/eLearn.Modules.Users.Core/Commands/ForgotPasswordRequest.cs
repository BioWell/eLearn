using System.ComponentModel.DataAnnotations;
using MediatR;

namespace eLearn.Modules.Users.Core.Commands
{
    internal class ForgotPasswordRequest : IRequest
    {
        [Required] [EmailAddress] public string Email { get; set; } = string.Empty;
    }
}