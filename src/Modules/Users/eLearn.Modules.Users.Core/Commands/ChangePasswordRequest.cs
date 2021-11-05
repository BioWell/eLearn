using System;
using System.ComponentModel.DataAnnotations;

namespace eLearn.Modules.Users.Core.Commands
{
    // no need data annotations remove it
    public class ChangePasswordRequest
    {
        [Required(ErrorMessage = "The {0} field is required.")]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; } = String.Empty;

        [Required(ErrorMessage = "The {0} field is required.")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.",
            MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; } = String.Empty;

        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; } = String.Empty;
    }
}