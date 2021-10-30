using eLearn.Modules.Users.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eLearn.Modules.Users.Core.Persistence.Configurations
{
    internal class StateOrProvinceCfg :IEntityTypeConfiguration<StateOrProvince>
    {
        public void Configure(EntityTypeBuilder<StateOrProvince> builder)
        {
            builder.Property(e => e.Code).HasMaxLength(450);
            builder.Property(e => e.Type).HasMaxLength(450);
            builder.Property(e => e.Name).HasMaxLength(450).IsRequired();
            
            builder.ToTable("Core_StateOrProvince");
        }
    }
}