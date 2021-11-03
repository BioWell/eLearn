using System;
using System.Reflection;
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
        public static IServiceCollection AddCore(this IServiceCollection services, string moduleName)
        {
            services.AddHttpContextAccessor();
            services.AddMediatR(Assembly.GetExecutingAssembly());
            // services.AddAutoMapper(Assembly.GetExecutingAssembly());
            var registrationOptions = services.GetOptions<RegistrationSettings>($"{moduleName}:RegistrationSettings");
            services.AddSingleton(registrationOptions);
            services.AddDatabase()
                .AddScoped<IUserRepository, UserRepository>();
            services.AddTransient<IIdentityService, IdentityService>();
            services.AddTransient<ITokenService, TokenService>();
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            return services;
        }

        private static IServiceCollection AddDatabase(this IServiceCollection services)
        {
            var options = services.GetOptions<MsSqlSettings>(nameof(MsSqlSettings));
            services.AddDbContext<UsersDbContext>(optionsBuilder =>
            {
                // optionsBuilder.EnableSensitiveDataLogging(true);
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
                    
                    identityOptions.User.RequireUniqueEmail = true;

                    identityOptions.SignIn.RequireConfirmedEmail = true; 
                    // TO DO
                    // identityOptions.Tokens.EmailConfirmationTokenProvider = "emailconf";
                    identityOptions.Lockout.AllowedForNewUsers = true;
                    identityOptions.Lockout.MaxFailedAccessAttempts = 3;
                    identityOptions.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);
                })
                .AddEntityFrameworkStores<UsersDbContext>()
                .AddDefaultTokenProviders();
            
            // Develop purpose
            services.Configure<DataProtectionTokenProviderOptions>(options =>
                options.TokenLifespan = TimeSpan.FromMinutes(10));

            services.AddHangfire(x => x.UseSqlServerStorage(options.ConnectionString));
            return services;
        }
    }
}