using eLearn.Modules.Users.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eLearn.Modules.Users.Core.Persistence.Configurations
{
    internal class CountryCfg : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> builder)
        {
            builder.Property(e => e.Code3).HasMaxLength(450);
            builder.Property(e => e.Name).HasMaxLength(450);//.IsRequired();
            
            builder.HasMany(e => e.StatesOrProvinces)
                .WithOne(e => e.Country)
                .HasForeignKey(ur => ur.CountryId)
                .OnDelete(DeleteBehavior.Restrict);
            
            builder.ToTable("Core_Country");
        }
    }
}