using eLearn.Modules.Users.Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace eLearn.Modules.Users.Core.Persistence
{
    internal class UsersDbContext : IdentityDbContext
    {
        public DbSet<ELearnUser> ELearnUsers { get; set; } ;
        
        public DbSet<Organization> Organizations { get; set; }
        
        public UsersDbContext(DbContextOptions<UsersDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.HasDefaultSchema("users");
            builder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }
    }
}