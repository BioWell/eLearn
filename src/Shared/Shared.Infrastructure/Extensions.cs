﻿using System.Reflection;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Infrastructure.Api;
using Shared.Infrastructure.Exceptions;
using Shared.Infrastructure.Services.Email;

namespace Shared.Infrastructure
{
    public static class Extensions
    {
        public static IServiceCollection AddModularInfrastructure(this IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddErrorHandling();
            services.AddControllers()
                .ConfigureApplicationPartManager(manager =>
                {
                    manager.FeatureProviders.Add(new InternalControllerFeatureProvider());
                });
            services.AddLocalization(options =>
            {
                options.ResourcesPath = "Resources";
            });
            services.AddApplicationLayer();
            services.AddEndpointsApiExplorer();
            return services;
        }

        public static WebApplication UseModularInfrastructure(this WebApplication app)
        {
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapGet("/", context => context.Response.WriteAsync("eLearn API"));
            });
            return app;
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

        private static IServiceCollection AddApplicationLayer(this IServiceCollection services)
        {
            var options = services.GetOptions<MailSettings>(nameof(MailSettings));
            
            services.AddTransient<IMailService, SmtpMailService>();
            services.AddSingleton(options);
            return services;
        }
    }
}