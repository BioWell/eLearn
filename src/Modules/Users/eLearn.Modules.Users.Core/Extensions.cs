using eLearn.Modules.Users.Core.Entities;
using eLearn.Modules.Users.Core.Persistence;
using eLearn.Modules.Users.Core.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace eLearn.Modules.Users.Core
{
    internal static class Extensions
    {
        public static IServiceCollection AddCore(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>()
                .AddIdentity<ELearnUser, IdentityRole>()
                .AddEntityFrameworkStores<UsersDbContext>();

            return services;
        }        
    }
}