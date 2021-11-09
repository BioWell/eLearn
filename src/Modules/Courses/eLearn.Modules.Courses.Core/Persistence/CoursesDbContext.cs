using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace eLearn.Modules.Courses.Core.Persistence
{
    internal sealed class CoursesDbContext : DbContext
    {
        internal string Schema => "Courses";

        public CoursesDbContext(DbContextOptions<CoursesDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.HasDefaultSchema(Schema);
            base.OnModelCreating(builder);
            ApplyIdentityConfiguration(builder);
        }

        private void ApplyIdentityConfiguration(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(GetType().Assembly);

            foreach (var property in builder.Model.GetEntityTypes()
                .SelectMany(t => t.GetProperties())
                .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
            {
                property.SetColumnType("decimal(23,2)");
            }
        }
    }
}