using eLearn.Modules.Users.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eLearn.Modules.Users.Core.Persistence.Configurations
{
    internal class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Core_User");
            builder.HasIndex(x => x.Email).IsUnique();
            builder.Property(x => x.Email).IsRequired().HasMaxLength(450);
            builder.Property(x => x.RefreshTokenHash).HasMaxLength(450);
            builder.Property(x => x.Culture).HasMaxLength(450);
            
            builder.HasOne(x => x.DefaultShippingAddress)
                .WithMany()
                .HasForeignKey(x => x.DefaultShippingAddressId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.DefaultBillingAddress)
                .WithMany()
                .HasForeignKey(x => x.DefaultBillingAddressId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}