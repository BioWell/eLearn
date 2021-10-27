using eLearn.Modules.Users.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eLearn.Modules.Users.Core.Persistence.Configurations
{
    internal class UserConfiguration : IEntityTypeConfiguration<ELearnUser>
    {
        public void Configure(EntityTypeBuilder<ELearnUser> builder)
        {
            builder.HasIndex(x => x.Email).IsUnique();
            builder.Property(x => x.Email).IsRequired().HasMaxLength(500);
            builder.HasIndex(x => x.Locale).IsUnique(false);
        }
    }
}