using System.Reflection;
using System.Runtime.CompilerServices;
using eLearn.Modules.Users.Core.Entities;
using eLearn.Modules.Users.Core.Persistence;
using eLearn.Modules.Users.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shared.Infrastructure;
using Shared.Infrastructure.Persistence.SqlServer;
using MediatR;

[assembly: InternalsVisibleTo("eLearn.Modules.Users.Api")]

namespace eLearn.Modules.Users.Core
{
    internal static class Extensions
    {
        public static IServiceCollection AddCore(this IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());
            var registrationOptions = services.GetOptions<RegistrationOptions>("users:registration");
            services.AddSingleton(registrationOptions);
            services.AddDatabase()
                .AddScoped<IUserRepository, UserRepository>();

            return services;
        }

        private static IServiceCollection AddDatabase(this IServiceCollection services)
        {
            var options = services.GetOptions<SqlserverOptions>(nameof(SqlserverOptions));
            services.AddDbContext<UsersDbContext>(optionsBuilder =>
            {
                optionsBuilder.EnableSensitiveDataLogging(true);
                optionsBuilder.UseSqlServer(options.ConnectionString);
            });

            services.AddIdentity<AppUser, AppRole>(identityOptions =>
                {
                    identityOptions.Password.RequireDigit = false;
                    identityOptions.Password.RequiredLength = 4;
                    identityOptions.Password.RequireNonAlphanumeric = false;
                    identityOptions.Password.RequireUppercase = false;
                    identityOptions.Password.RequireLowercase = false;
                    identityOptions.Password.RequiredUniqueChars = 0;
                })
                .AddEntityFrameworkStores<UsersDbContext>();

            return services;
        }
    }
}