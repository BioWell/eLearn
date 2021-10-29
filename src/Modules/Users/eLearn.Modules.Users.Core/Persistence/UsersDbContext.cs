﻿using System.ComponentModel.DataAnnotations;
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

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            ValidateEntities();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ValidateEntities();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // builder.HasDefaultSchema("users");
            builder.ApplyConfigurationsFromAssembly(GetType().Assembly);
            CoreCustomEntitiesBuilder(builder);
            base.OnModelCreating(builder);
        }

        private void CoreCustomEntitiesBuilder(ModelBuilder builder)
        {
            builder.Entity<IdentityUserClaim<long>>(b =>
            {
                b.HasKey(uc => uc.Id);
                b.ToTable("Core_UserClaim");
            });

            builder.Entity<IdentityRoleClaim<long>>(b =>
            {
                b.HasKey(rc => rc.Id);
                b.ToTable("Core_RoleClaim");
            });

            builder.Entity<IdentityUserLogin<long>>(b => { b.ToTable("Core_UserLogin"); });
            builder.Entity<IdentityUserToken<long>>(b => { b.ToTable("Core_UserToken"); });
            builder.Entity<IdentityUserLogin<long>>(b => { b.ToTable("Core_UserLogin"); });
            builder.Entity<IdentityUserToken<long>>(b => { b.ToTable("Core_UserToken"); });
        }

        private void ValidateEntities()
        {
            var modifiedEntries = ChangeTracker.Entries()
                .Where(x => (x.State == EntityState.Added || x.State == EntityState.Modified));

            foreach (var entity in modifiedEntries)
            {
                if (entity.Entity is ValidatableObject validatableObject)
                {
                    var validationResults = validatableObject.Validate();
                    if (validationResults.Any())
                    {
                        //throw new ValidationException((string)entity.Entity.GetType(), validationResults);
                    }
                }
            }
        }
    }
}