using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Shared.Infrastructure.Cors
{
    public static class Extensions
    {
        public static IServiceCollection AddCorsPolicy(this IServiceCollection services)
        {
            var corsOptions = services.GetOptions<CorsSettings>(nameof(CorsSettings));

            return services
                .AddSingleton(corsOptions)
                .AddCors(cors =>
                {
                    var allowedHeaders = corsOptions.AllowedHeaders ?? Enumerable.Empty<string>();
                    var allowedMethods = corsOptions.AllowedMethods ?? Enumerable.Empty<string>();
                    var allowedOrigins = corsOptions.AllowedOrigins ?? Enumerable.Empty<string>();
                    var exposedHeaders = corsOptions.ExposedHeaders ?? Enumerable.Empty<string>();
                    cors.AddPolicy("cors", corsBuilder =>
                    {
                        var origins = allowedOrigins.ToArray();
                        if (corsOptions.AllowCredentials && origins.FirstOrDefault() != "*")
                        {
                            corsBuilder.AllowCredentials();
                        }
                        else
                        {
                            corsBuilder.DisallowCredentials();
                        }

                        corsBuilder.WithHeaders(allowedHeaders.ToArray())
                            .WithMethods(allowedMethods.ToArray())
                            .WithOrigins(origins.ToArray())
                            .WithExposedHeaders(exposedHeaders.ToArray());
                    });
                });
        }
    }
}