using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using eLearn.Modules.Courses.Core.Persistence;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shared.Infrastructure;
using Shared.Infrastructure.Persistence.SqlServer;

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
            var options = services.GetOptions<MsSqlSettings>(nameof(MsSqlSettings));
            services
                .AddDbContext<CoursesDbContext>(m => m.UseSqlServer(options.ConnectionString))
                .AddScoped<ICoursesDbContext>(provider => provider.GetService<ICoursesDbContext>() ?? throw new InvalidOperationException());
            return services;
        } 
    }
}