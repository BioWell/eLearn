using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Shared.Infrastructure.Swagger.Filters;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Shared.Infrastructure.Swagger
{
    public static class Extensions
    {
        public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
        {
            var swaggerOptions = services.GetOptions<SwaggerSettings>(nameof(SwaggerSettings));
            services.AddSingleton(swaggerOptions);
            if (swaggerOptions.Enabled == true)
            {
                services.AddSwaggerGen(options =>
                {
                    string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                    foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
                    {
                        if (!assembly.IsDynamic)
                        {
                            string xmlFile = $"{assembly.GetName().Name}.xml";
                            string xmlPath = Path.Combine(baseDirectory, xmlFile);
                            if (File.Exists(xmlPath))
                            {
                                options.IncludeXmlComments(xmlPath);
                            }
                        }
                    }

                    options.AddSwaggerDocs(swaggerOptions);
                    options.OperationFilter<SwaggerExcludeFilter>();
                    
                    if (swaggerOptions.IncludeSecurity)
                    {
                        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                        {
                            Name = "Authorization",
                            In = ParameterLocation.Header,
                            Type = SecuritySchemeType.ApiKey,
                            Scheme = "Bearer",
                            BearerFormat = "JWT",
                            Description =
                                "Input your Bearer token in this format - Bearer {your token here} to access this API",
                        });
                        options.AddSecurityRequirement(new OpenApiSecurityRequirement
                        {
                            {
                                new OpenApiSecurityScheme
                                {
                                    Reference = new OpenApiReference
                                    {
                                        Type = ReferenceType.SecurityScheme,
                                        Id = "Bearer",
                                    },
                                    Scheme = "Bearer",
                                    Name = "Bearer",
                                    In = ParameterLocation.Header,
                                },
                                new List<string>()
                            },
                        });
                    }

                    if (swaggerOptions.SerializeAsOpenApiV2)
                    {
                        options.MapType<TimeSpan>(() => new OpenApiSchema
                        {
                            Type = "string",
                            Nullable = true,
                            Pattern =
                                @"^([0-9]{1}|(?:0[0-9]|1[0-9]|2[0-3])+):([0-5]?[0-9])(?::([0-5]?[0-9])(?:.(\d{1,9}))?)?$",
                            Example = new OpenApiString("02:00:00")
                        });
                    }
                });
            }

            return services;
        }

        private static void AddSwaggerDocs(this SwaggerGenOptions options, SwaggerSettings settings)
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = settings.Version,
                Title = settings.Title,
                License = new OpenApiLicense
                {
                    Name = "MIT License",
                    Url = new Uri("https://opensource.org/licenses/MIT")
                }
            });
        }

        public static WebApplication UseSwaggerDocumentation(this WebApplication app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.DefaultModelsExpandDepth(-1);
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                options.RoutePrefix = "swagger";
                options.DisplayRequestDuration();
                options.DocExpansion(DocExpansion.None);
            });
            return app;
        }
    }
}