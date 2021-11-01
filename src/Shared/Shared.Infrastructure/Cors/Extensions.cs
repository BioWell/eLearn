using Microsoft.Extensions.DependencyInjection;

namespace Shared.Infrastructure.Cors
{
    public static class Extensions
    {
        public static IServiceCollection AddCorsPolicy(this IServiceCollection services)
        {
            var corsSettings = services.GetOptions<CorsSettings>(nameof(CorsSettings));
            return services.AddCors(opt =>
            {
                opt.AddPolicy("CorsPolicy", policy =>
                {
                    policy.AllowAnyHeader().AllowAnyMethod().WithOrigins(corsSettings.Url);
                });
            });
        }
    }
}