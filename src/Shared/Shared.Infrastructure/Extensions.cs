using System.IO;
using System.Net;
using System.Reflection;
using FluentValidation;
using FluentValidation.AspNetCore;
using Hangfire;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Shared.Infrastructure.Api;
using Shared.Infrastructure.Auth;
using Shared.Infrastructure.Caching;
using Shared.Infrastructure.Cors;
using Shared.Infrastructure.Exceptions;
using Shared.Infrastructure.Hangfire;
using Shared.Infrastructure.Interceptors;
using Shared.Infrastructure.Serialization;
using Shared.Infrastructure.Services.Email;
using Shared.Infrastructure.Swagger;

namespace Shared.Infrastructure
{
    public static class Extensions
    {
        public static IServiceCollection AddModularInfrastructure(this IServiceCollection services)
        {
            services.AddDistributedMemoryCache()
                .AddSerialization()
                .AddSharedInfrastructure()
                .AddSharedApplication();
            return services;
        }

        public static WebApplication UseModularInfrastructure(this WebApplication app)
        {
            app.UseMiddleware<GlobalExceptionHandler>();
            app.UseRouting();

            string filesDirectoryPath = Path.Combine(Directory.GetCurrentDirectory(), "Files");
            if (!Directory.Exists(filesDirectoryPath))
            {
                Directory.CreateDirectory(filesDirectoryPath);
            }

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Files")),
                RequestPath = "/files"
            });
            app.UseCors("CorsPolicy");
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseHangfireDashboard("/jobs", new DashboardOptions
            {
                DashboardTitle = "eLearn Jobs"
            });
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapGet("/", context => context.Response.WriteAsync("eLearn API"));
            });
            app.UseSwaggerDocumentation();
            return app;
        }

        private static IServiceCollection AddSharedInfrastructure(this IServiceCollection services)
        {
            // 1. Controller MediatR & AutoMapper & Application layer
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddApplicationLayer();
            
            // 2. DbSet 
            //   Domain event & log
            // ...

            // 3. Controller
            services.AddControllers()
                .ConfigureApplicationPartManager(manager =>
                {
                    manager.FeatureProviders.Add(new InternalControllerFeatureProvider());
                })
                .AddMvcOptions(options =>
                {
                    options.ModelBindingMessageProvider.SetAttemptedValueIsInvalidAccessor((value, propertyName) =>
                        throw new CustomException($"{propertyName}: value '{value}' is invalid.",
                            statusCode: HttpStatusCode.BadRequest));
                });

            // 4. Localize
            services.AddTransient<IValidatorInterceptor, ValidatorInterceptor>();
            services.AddLocalization(options => { options.ResourcesPath = "Resources"; });

            // 5. Background services
            services.AddEndpointsApiExplorer();
            services.AddHangfireServer();

            // 6. Doc & swagger
            services.AddSwaggerDocumentation();
            
            // 7. Error Handler
            services.AddSingleton<GlobalExceptionHandler>();
            services.AddCorsPolicy();
            return services;
        }

        private static IServiceCollection AddSharedApplication(this IServiceCollection services)
        {
            var cachOptions = services.GetOptions<CacheSettings>(nameof(CacheSettings));
            services.AddSingleton(cachOptions);
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(CachingBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            return services;
        }

        private static IServiceCollection AddApplicationLayer(this IServiceCollection services)
        {
            var appOptions = services.GetOptions<AppSettings>(nameof(AppSettings));
            services.AddSingleton(appOptions);
            var mailOptions = services.GetOptions<MailSettings>(nameof(MailSettings));
            services.AddSingleton(mailOptions);
            var authOptions = services.GetOptions<AuthSettings>(nameof(AuthSettings));
            services.AddSingleton(authOptions);
            services.AddTransient<IMailService, SmtpMailService>();
            services.AddScoped<IJobService, HangfireService>();
            return services;
        }

        public static T GetOptions<T>(this IServiceCollection services, string sectionName) where T : new()
        {
            using var serviceProvider = services.BuildServiceProvider();
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            return configuration.GetOptions<T>(sectionName);
        }

        public static T GetOptions<T>(this IConfiguration configuration, string sectionName) where T : new()
        {
            var options = new T();
            configuration.GetSection(sectionName).Bind(options);
            return options;
        }
    }
}