using eLearn.Modules.Users.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eLearn.Modules.Users.Core.Persistence.Configurations
{
    internal class UserCfg : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            builder.Property(x => x.Culture).HasMaxLength(450);
            builder.Property(x => x.RefreshTokenHash).HasMaxLength(450);
            builder.Property(x => x.FullName).IsRequired().HasMaxLength(450);

            // builder.Property(x => x.ConcurrencyStamp).IsConcurrencyToken();
            // builder.Property(x => x.Email).HasMaxLength(256);
            // builder.HasIndex(x => x.NormalizedEmail).IsUnique().HasDatabaseName("UserNameIndex")
            //     .HasFilter("[NormalizedUserName] IS NOT NULL");
            // builder.Property(x => x.NormalizedEmail).HasMaxLength(256);
            // builder.Property(x => x.NormalizedUserName).HasMaxLength(256);
            // builder.Property(x => x.UserName).HasMaxLength(256);

            // builder.HasOne(d => d.DefaultBillingAddress)
            //     .WithMany()
            //     .HasForeignKey(f => f.DefaultBillingAddressId)
            //     .OnDelete(DeleteBehavior.Restrict);
            // builder.HasOne(d => d.DefaultShippingAddress)
            //     .WithMany()
            //     .HasForeignKey(f => f.DefaultShippingAddressId)
            //     .OnDelete(DeleteBehavior.Restrict);
        }
    }
}