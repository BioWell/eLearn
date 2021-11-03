using System.ComponentModel.DataAnnotations;
using MediatR;

namespace eLearn.Modules.Users.Core.Commands
{
    internal class ResetPasswordRequest : IRequest
    {
        [Required(ErrorMessage = "The {0} field is required.")]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "The {0} field is required.")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.",
            MinimumLength = 4)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "The {0} field is required.")]
        public string Token { get; set; } = string.Empty;
    }
}