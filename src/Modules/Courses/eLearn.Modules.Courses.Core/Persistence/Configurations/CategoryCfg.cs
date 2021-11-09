using eLearn.Modules.Courses.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eLearn.Modules.Courses.Core.Persistence.Configurations
{
    internal class CategoryCfg : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("Core_Category");
        }
    }
}