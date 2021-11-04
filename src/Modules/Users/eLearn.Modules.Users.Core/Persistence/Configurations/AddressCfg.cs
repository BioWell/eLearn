using eLearn.Modules.Users.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eLearn.Modules.Users.Core.Persistence.Configurations
{
    internal class AddressCfg : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.Property(e => e.ContactName).HasMaxLength(450);
            builder.Property(e => e.Phone).HasMaxLength(450);
            builder.Property(e => e.AddressLine1).HasMaxLength(450);
            builder.Property(e => e.AddressLine2).HasMaxLength(450);
            builder.Property(e => e.City).HasMaxLength(450);
            builder.Property(e => e.ZipCode).HasMaxLength(450);

            builder.HasOne(o => o.Country)
                .WithMany()
                .HasForeignKey(f => f.CountryId)
                .OnDelete(DeleteBehavior.Restrict);
            
            builder.HasOne(o => o.StateOrProvince)
                .WithMany()
                .HasForeignKey(f => f.StateOrProvinceId)
                .OnDelete(DeleteBehavior.Restrict);
            
            builder.HasOne(o => o.District)
                .WithMany()
                .HasForeignKey(f => f.DistrictId)
                .OnDelete(DeleteBehavior.Restrict);
            
            builder.ToTable("Core_Address");
        }
    }
}