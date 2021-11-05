using System;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using eLearn.Modules.Users.Core.Entities;
using eLearn.Modules.Users.Core.Exceptions;
using eLearn.Modules.Users.Core.Permissions;
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
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Shared.Infrastructure.Auth;

[assembly: InternalsVisibleTo("eLearn.Modules.Users.Api")]

namespace eLearn.Modules.Users.Core
{
    internal static class Extensions
    {
        public static IServiceCollection AddCore(this IServiceCollection services, string moduleName)
        {
            services.AddHttpContextAccessor();
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            var registrationOptions =
                services.GetOptions<RegistrationSettings>($"{moduleName}:" + nameof(RegistrationSettings));
            services.AddSingleton(registrationOptions);

            services.AddTransient<IIdentityService, IdentityService>()
                .AddTransient<IRoleService, RoleService>()
                .AddTransient<IUserService, UserService>()
                .AddTransient<ICurrentUser, CurrentUser>()
                .AddTransient<IRoleClaimService, RoleClaimService>()
                .AddTransient<ITokenService, TokenService>();

            services.AddDatabase()
                .AddScoped<IUserRepository, UserRepository>();

            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            return services;
        }

        private static IServiceCollection AddDatabase(this IServiceCollection services)
        {
            var options = services.GetOptions<MsSqlSettings>(nameof(MsSqlSettings));
            services.AddDbContext<UsersDbContext>(optionsBuilder =>
                {
                    //optionsBuilder.EnableSensitiveDataLogging(true);
                    optionsBuilder.UseSqlServer(options.ConnectionString);
                })
                .AddScoped<IUsersDbContext>(provider => provider.GetService<UsersDbContext>() ?? throw new InvalidOperationException())
                .AddIdentity<AppUser, AppRole>(identityOptions =>
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

            services.AddPermissions();
            services.AddJwtAuthentication();

            // Develop purpose
            services.Configure<DataProtectionTokenProviderOptions>(options =>
                options.TokenLifespan = TimeSpan.FromMinutes(10));

            services.AddHangfire(x => x.UseSqlServerStorage(options.ConnectionString));
            return services;
        }

        private static IServiceCollection AddPermissions(this IServiceCollection services)
        {
            services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>()
                .AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();
            return services;
        }

        private static IServiceCollection AddJwtAuthentication(this IServiceCollection services)
        {
            var jwtSettings = services.GetOptions<AuthSettings>(nameof(AuthSettings));
            byte[] key = Encoding.ASCII.GetBytes(jwtSettings.Key);
            services
                .AddAuthentication(authentication =>
                {
                    authentication.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    authentication.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(bearer =>
                {
                    bearer.RequireHttpsMetadata = false;
                    bearer.SaveToken = true;
                    bearer.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        RoleClaimType = ClaimTypes.Role,
                        ClockSkew = TimeSpan.Zero
                    };
                    bearer.Events = new JwtBearerEvents
                    {
                        OnChallenge = context =>
                        {
                            context.HandleResponse();
                            if (!context.Response.HasStarted)
                            {
                                throw new IdentityException("You are not Authorized.",
                                    statusCode: HttpStatusCode.Unauthorized);
                            }

                            return Task.CompletedTask;
                        },
                        OnForbidden = context =>
                        {
                            throw new IdentityException("You are not authorized to access this resource.",
                                statusCode: HttpStatusCode.Forbidden);
                        },
                    };
                });
            return services;
        }
    }
}