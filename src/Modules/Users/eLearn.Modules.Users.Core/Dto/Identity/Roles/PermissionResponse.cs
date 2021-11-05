using System;
using System.Collections.Generic;

namespace eLearn.Modules.Users.Core.Dto.Identity.Roles
{
    internal class PermissionResponse
    {
        public string RoleId { get; set; } = String.Empty;

        public string RoleName { get; set; } = String.Empty;

        public List<RoleClaimModel>? RoleClaims { get; set; }
    }
}