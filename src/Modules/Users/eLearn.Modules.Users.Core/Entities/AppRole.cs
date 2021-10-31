using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Shared.Infrastructure.Entities;

namespace eLearn.Modules.Users.Core.Entities
{
    internal class AppRole : IdentityRole<long>, IEntityWithTypedId<long>
    {
        public virtual IList<AppUserRole>? UserRoles { get; set; }
    }
}