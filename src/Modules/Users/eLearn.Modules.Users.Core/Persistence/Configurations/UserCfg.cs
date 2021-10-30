using eLearn.Modules.Users.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eLearn.Modules.Users.Core.Persistence.Configurations
{
    internal class UserCfg : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(x => x.Culture).HasMaxLength(450);
            builder.Property(x => x.RefreshTokenHash).HasMaxLength(450);
            builder.Property(x => x.FullName).IsRequired().HasMaxLength(450);
            
            builder.HasMany(e => e.Roles)
                .WithOne(e => e.User)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();

            builder.HasMany(e => e.UserAddresses)
                .WithOne(e => e.User)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();
            
            builder.ToTable("Core_User");
            
            // builder.HasOne(x => x.DefaultShippingAddress)
            //     .with(x => x.DefaultBillingAddressId)
            //     .OnDelete(DeleteBehavior.Restrict);

            // builder.HasOne(x => x.DefaultBillingAddress)
            //     .WithMany()
            //     .HasForeignKey(x => x.DefaultBillingAddressId)
            //     .OnDelete(DeleteBehavior.Restrict);
        }
    }
}