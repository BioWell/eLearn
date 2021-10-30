using eLearn.Modules.Users.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eLearn.Modules.Users.Core.Persistence.Configurations
{
    internal class DistrictCfg : IEntityTypeConfiguration<District>
    {
        public void Configure(EntityTypeBuilder<District> builder)
        {
            builder.Property(e => e.Type).HasMaxLength(450);
            builder.Property(e => e.Name).HasMaxLength(450).IsRequired();

            builder.HasOne(o => o.StateOrProvince)
                .WithMany()
                .HasForeignKey(f => f.StateOrProvinceId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.ToTable("Core_District");
        }
    }
}