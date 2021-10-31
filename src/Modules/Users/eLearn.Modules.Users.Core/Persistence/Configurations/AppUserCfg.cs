using eLearn.Modules.Users.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eLearn.Modules.Users.Core.Persistence.Configurations
{
    internal class AppUserCfg : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.Property(x => x.Culture).HasMaxLength(450);
            builder.Property(x => x.RefreshTokenHash).HasMaxLength(450);
            builder.Property(x => x.FullName).IsRequired().HasMaxLength(450);
            
            builder.HasMany(x => x.UserRoles)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserId)
                .IsRequired();

            builder.HasMany(x => x.Addresses)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserId);
            
            // builder.HasOne(x => x.DefaultShippingAddress)
            //     .WithMany()
            //     .HasForeignKey(x => x.DefaultShippingAddressId)
            //     .OnDelete(DeleteBehavior.Restrict);
            //
            // builder.HasOne(x => x.DefaultBillingAddress)
            //     .WithMany()
            //     .HasForeignKey(x => x.DefaultBillingAddressId)
            //     .OnDelete(DeleteBehavior.Restrict);

            builder.ToTable("Core_User");
        }
    }
}