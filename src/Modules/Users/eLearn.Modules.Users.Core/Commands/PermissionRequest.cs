using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using eLearn.Modules.Users.Core.Dto.Identity.Roles;

namespace eLearn.Modules.Users.Core.Commands
{
    internal class PermissionRequest
    {
        [Required(ErrorMessage = "The {0} field is required.")]
        public string RoleId { get; set; } = String.Empty;

        public IList<RoleClaimModel>? RoleClaims { get; set; }
    }
}