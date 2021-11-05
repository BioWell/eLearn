using System;
using System.Collections.Generic;

namespace eLearn.Modules.Users.Core.Dto.Users
{
    internal class UserRolesResponse
    {
        public List<UserRoleModel> UserRoles { get; set; } = new();
    }

    internal class UserRoleModel
    {
        public string RoleId { get; set; } = String.Empty;

        public string RoleName { get; set; } = String.Empty;

        public bool Selected { get; set; }
    }
}