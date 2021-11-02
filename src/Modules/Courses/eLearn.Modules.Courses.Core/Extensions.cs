using System.Reflection;
using System.Runtime.CompilerServices;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

[assembly: InternalsVisibleTo("eLearn.Modules.Courses.Api")]

namespace eLearn.Modules.Courses.Core
{
    internal static class Extensions
    {
        public static IServiceCollection AddCore(this IServiceCollection services, string moduleName)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            AddDatabase(services);
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            return services;
        }       
        
        public static IServiceCollection AddDatabase(this IServiceCollection services)
        {
            return services;
        } 
    }
}