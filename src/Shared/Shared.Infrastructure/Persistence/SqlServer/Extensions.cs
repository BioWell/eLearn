using Microsoft.Extensions.DependencyInjection;

namespace Shared.Infrastructure.Persistence.SqlServer
{
    public static class Extensions
    {
        public static IServiceCollection AddPostgres(this IServiceCollection services)
        {
            var options = services.GetOptions<SqlserverOptions>("sqlserver");
            services.AddSingleton(options);
            return services;
        }
        
        // public static IServiceCollection AddPostgres<T>(this IServiceCollection services) where T : DbContext
        // {
        //     var options = services.GetOptions<SqlserverOptions>("postgres");
        //     services.AddDbContext<T>(x => x.UseNpgsql(options.ConnectionString));
        //
        //     return services;
        // }
    }
}