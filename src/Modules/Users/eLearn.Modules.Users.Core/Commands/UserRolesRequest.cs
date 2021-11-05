using System.Collections.Generic;
using eLearn.Modules.Users.Core.Dto.Users;

namespace eLearn.Modules.Users.Core.Commands
{
    internal class UserRolesRequest
    {
        public List<UserRoleModel> UserRoles { get; set; } = new();
    }
}