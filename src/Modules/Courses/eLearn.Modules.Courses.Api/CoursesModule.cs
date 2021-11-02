using System.Collections.Generic;
using eLearn.Modules.Courses.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Infrastructure.Modules;

namespace eLearn.Modules.Courses.Api
{
    public class CoursesModule: IModule
    {
        public string Name { get; } = "Courses";
        
        public IEnumerable<string> Policies { get; } = new[]
        {
            "course"
        };

        public void Register(IServiceCollection services)
        {
            services.AddCore(Name);
        }
        
        public void Use(IConfiguration app)
        {
        }
    }
}