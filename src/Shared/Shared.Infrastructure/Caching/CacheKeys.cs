using Shared.Infrastructure.Entities;
using Shared.Infrastructure.Utilities;

namespace Shared.Infrastructure.Caching
{
    public static class CacheKeys
    {
        public static class Common
        {
            public static string GetEntityByIdCacheKey<TEntityId, TEntity>(TEntityId id)
                where TEntity : class, IEntity<TEntityId>
            {
                return $"GetEntity-{typeof(TEntity).GetGenericTypeName()}-{id}";
            }
        }
    }
}