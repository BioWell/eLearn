using System;
using System.ComponentModel.DataAnnotations;

namespace eLearn.Modules.Users.Core.Commands
{
    internal class UpdateUserRequest
    {
        [Required(ErrorMessage = "The {0} field is required.")]
        public string Id { get; set; } = String.Empty;

        [Required(ErrorMessage = "The {0} field is required.")]
        public string FirstName { get; set; } = String.Empty;

        [Required(ErrorMessage = "The {0} field is required.")]
        public string LastName { get; set; } = String.Empty;

        [Required(ErrorMessage = "The {0} field is required.")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.",
            MinimumLength = 4)]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; } = String.Empty;
        
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.",
            MinimumLength = 4)]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Compare("Password")] 
        public string? ConfirmPassword { get; set; }

        public string? PhoneNumber { get; set; }
    }
}