using eLearn.Modules.Users.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eLearn.Modules.Users.Core.Persistence.Configurations
{
    internal class UserRoleCfg : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.HasKey(ur => new {ur.UserId, ur.RoleId});
            builder.HasOne(ur => ur.Role)
                .WithMany(x => x.Users)
                .HasForeignKey(r => r.RoleId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(ur => ur.User)
                .WithMany(x => x.Roles)
                .HasForeignKey(u => u.UserId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.ToTable("Core_UserRole");
        }
    }
}