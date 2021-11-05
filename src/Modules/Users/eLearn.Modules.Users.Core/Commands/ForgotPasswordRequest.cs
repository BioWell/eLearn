using System;
using System.ComponentModel.DataAnnotations;
using MediatR;

namespace eLearn.Modules.Users.Core.Commands
{
    internal class ForgotPasswordRequest : IRequest
    {
        [Required(ErrorMessage = "The {0} field is required.")]
        [EmailAddress]
        public string Email { get; set; } = String.Empty;
    }
}