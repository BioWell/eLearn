using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Shared.Infrastructure.Entities;

namespace eLearn.Modules.Users.Core.Entities
{
    internal class Role : IdentityRole<long>, IEntityWithTypedId<long>
    {
        public virtual IList<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
}