using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Shared.Infrastructure.Serialization.Settings;

namespace Shared.Infrastructure.Serialization
{
    public static class Extensions
    {
        public static IServiceCollection AddSerialization(this IServiceCollection services)
        {
            var options = services.GetOptions<SerializationSettings>(nameof(SerializationSettings));
            services.AddSingleton(options);
            services.AddSingleton<IJsonSerializerSettingsOptions, JsonSerializerSettingsOptions>();
            if (options.UseSystemTextJson)
            {
                services.AddSingleton<IJsonSerializer, SystemTextJsonSerializer>()
                    .Configure<JsonSerializerSettingsOptions>(configureOptions =>
                    {
                        if (configureOptions.JsonSerializerOptions != null && 
                            configureOptions.JsonSerializerOptions.Converters.All(c =>
                            c.GetType() != typeof(TimespanJsonConverter)))
                        {
                            configureOptions.JsonSerializerOptions.Converters.Add(new TimespanJsonConverter());
                        }
                    });
            }
            else if (options.UseNewtonsoftJson)
            {
                services
                    .AddSingleton<IJsonSerializer, NewtonSoftJsonSerializer>();
            }

            return services;
        }
    }
}