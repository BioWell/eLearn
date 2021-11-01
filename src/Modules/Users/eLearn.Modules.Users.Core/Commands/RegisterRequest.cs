using System.ComponentModel.DataAnnotations;
using MediatR;

namespace eLearn.Modules.Users.Core.Commands
{
    internal class RegisterRequest : IRequest
    {
        [Required(ErrorMessage = "The {0} field is required.")]
        [EmailAddress] 
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;
        
        public bool EmailConfirmed { get; set; }
        
        [Required(ErrorMessage = "The {0} field is required.")]
        [Display(Name = "Name")]
        public string FullName { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "The {0} field is required.")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 4)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public bool PhoneNumberConfirmed { get; set; }

        public string? Role { get; set; }
    }
}