using eLearn.Modules.Users.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eLearn.Modules.Users.Core.Persistence.Configurations
{
    internal class UserAddressCfg : IEntityTypeConfiguration<UserAddress>
    {
        public void Configure(EntityTypeBuilder<UserAddress> builder)
        {
            builder.HasOne(ur => ur.Address)
                .WithMany(x => x.Users)
                .HasForeignKey(r => r.AddressId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(ur => ur.User)
                .WithMany(x => x.Addresses)
                .HasForeignKey(u => u.UserId)
                .OnDelete(DeleteBehavior.Restrict);
            
            builder.ToTable("Core_UserAddress");
        }
    }
}