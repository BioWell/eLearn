﻿using System.Reflection;
using System.Runtime.CompilerServices;
using eLearn.Modules.Users.Core.Entities;
using eLearn.Modules.Users.Core.Persistence;
using eLearn.Modules.Users.Core.Repositories;
using Microsoft.AspNetCore.Identity;
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

        public static IServiceCollection AddDatabase(this IServiceCollection services)
        {
            var options = services.GetOptions<SqlserverOptions>("sqlserver");
            services.AddDbContext<UsersDbContext>(optionsBuilder =>
            {
                optionsBuilder.UseSqlServer(options.ConnectionString);
            });

            services.AddIdentity<User, IdentityRole>(options =>
                {
                    options.Password.RequireDigit = false;
                    options.Password.RequiredLength = 4;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequiredUniqueChars = 0;
                })
                .AddEntityFrameworkStores<UsersDbContext>();

            return services;
        }
    }
}