using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Shared.Infrastructure.Entities;

namespace eLearn.Modules.Users.Core.Entities
{
    internal class AppRole : IdentityRole<long>, IEntityWithTypedId<long>
    {
        public string Description { get; set; } = String.Empty;
        public virtual IList<AppUserRole>? UserRoles { get; set; }
        public virtual IList<AppRoleClaim>? RoleClaims { get; set; }
        
        public AppRole()
            : base()
        {
        }
        
        public AppRole(string roleName, string? roleDescription)
            : base(roleName)
        {
            Description = roleDescription?? String.Empty;
        }
    }
}