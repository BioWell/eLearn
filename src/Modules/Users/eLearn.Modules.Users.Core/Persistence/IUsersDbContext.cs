using eLearn.Modules.Users.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace eLearn.Modules.Users.Core.Persistence
{
    internal interface IUsersDbContext
    {
        public DbSet<AppUser> Users { get; set; }

        public DbSet<AppRole> Roles { get; set; }

        public DbSet<AppRoleClaim> RoleClaims { get; set; }
    }
}