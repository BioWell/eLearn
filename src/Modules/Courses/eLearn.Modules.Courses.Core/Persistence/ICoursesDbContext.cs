using eLearn.Modules.Courses.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace eLearn.Modules.Courses.Core.Persistence
{
    internal interface ICoursesDbContext
    {
        public DbSet<Category> Categories { get; set; }
    }
}