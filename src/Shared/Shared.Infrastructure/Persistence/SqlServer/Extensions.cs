using Microsoft.Extensions.DependencyInjection;

namespace Shared.Infrastructure.Persistence.SqlServer
{
    public static class Extensions
    {
        public static IServiceCollection AddPostgres(this IServiceCollection services)
        {
            var options = services.GetOptions<MsSqlSettings>(nameof(MsSqlSettings));
            services.AddSingleton(options);
            return services;
        }
        
    }
}