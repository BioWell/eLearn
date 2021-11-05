using System;
using System.ComponentModel.DataAnnotations;

namespace eLearn.Modules.Users.Core.Commands
{
    internal class RoleClaimRequest
    {
        [Required(ErrorMessage = "The {0} field is required.")]
        public int Id { get; set; }

        [Required(ErrorMessage = "The {0} field is required.")]
        public string RoleId { get; set; } = String.Empty;

        [Required(ErrorMessage = "The {0} field is required.")]
        public string Type { get; set; } = String.Empty;

        [Required(ErrorMessage = "The {0} field is required.")]
        public string Value { get; set; } = String.Empty;

        public string? Description { get; set; }

        public string? Group { get; set; }
    }
}