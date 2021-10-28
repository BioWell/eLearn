using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;

[assembly: InternalsVisibleTo("eLearn.Modules.Courses.Api")]

namespace eLearn.Modules.Courses.Core
{
    internal static class Extensions
    {
        public static IServiceCollection AddCore(this IServiceCollection services)
        {
            AddDatabase(services);
            return services;
        }       
        
        public static IServiceCollection AddDatabase(this IServiceCollection services)
        {
            return services;
        } 
    }
}