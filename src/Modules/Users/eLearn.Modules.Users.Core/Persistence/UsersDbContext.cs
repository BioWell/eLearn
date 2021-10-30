using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using eLearn.Modules.Users.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Shared.Infrastructure.Entities;

namespace eLearn.Modules.Users.Core.Persistence
{
    internal class UsersDbContext : IdentityDbContext<User, Role, long,
        IdentityUserClaim<long>, UserRole, IdentityUserLogin<long>, IdentityRoleClaim<long>, IdentityUserToken<long>>
    {
        public UsersDbContext(DbContextOptions options) : base(options)
        {
        }

        // public override int SaveChanges(bool acceptAllChangesOnSuccess)
        // {
        //     ValidateEntities();
        //     return base.SaveChanges(acceptAllChangesOnSuccess);
        // }
        //
        // public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
        //     CancellationToken cancellationToken = default(CancellationToken))
        // {
        //     ValidateEntities();
        //     return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        // }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }
        
        // private void ValidateEntities()
        // {
        //     var modifiedEntries = ChangeTracker.Entries()
        //         .Where(x => (x.State == EntityState.Added || x.State == EntityState.Modified));
        //
        //     foreach (var entity in modifiedEntries)
        //     {
        //         if (entity.Entity is ValidatableObject validatableObject)
        //         {
        //             var validationResults = validatableObject.Validate();
        //             if (validationResults.Any())
        //             {
        //                 //throw new ValidationException((string)entity.Entity.GetType(), validationResults);
        //             }
        //         }
        //     }
        // }
    }
}