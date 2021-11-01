﻿using System.Reflection;
using System.Runtime.CompilerServices;
using eLearn.Modules.Users.Core.Entities;
using eLearn.Modules.Users.Core.Persistence;
using eLearn.Modules.Users.Core.Repositories;
using eLearn.Modules.Users.Core.Services;
using FluentValidation;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shared.Infrastructure;
using Shared.Infrastructure.Persistence.SqlServer;
using MediatR;
using Microsoft.AspNetCore.Identity;

[assembly: InternalsVisibleTo("eLearn.Modules.Users.Api")]

namespace eLearn.Modules.Users.Core
{
    internal static class Extensions
    {
        public static IServiceCollection AddCore(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddMediatR(Assembly.GetExecutingAssembly());
            // services.AddAutoMapper(Assembly.GetExecutingAssembly());
            var registrationOptions = services.GetOptions<RegistrationOptions>("users:registration");
            services.AddSingleton(registrationOptions);
            services.AddDatabase()
                .AddScoped<IUserRepository, UserRepository>();
            services.AddTransient<IIdentityService, IdentityService>();
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
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

                    identityOptions.SignIn.RequireConfirmedEmail = true;
                })
                .AddEntityFrameworkStores<UsersDbContext>()
                .AddDefaultTokenProviders();
            services.AddHangfire(x => x.UseSqlServerStorage(options.ConnectionString));
            return services;
        }
    }
}