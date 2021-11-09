using Shared.Infrastructure.Api.Contracts;

namespace Shared.Infrastructure.Api.Filters
{
    public class PaginatedExtendedAttributeFilter<TEntityId, TEntity> : PaginatedFilter
        where TEntity : class, IEntity<TEntityId>
    {
        public string? SearchString { get; set; }

        public TEntityId? EntityId { get; set; }

        public ExtendedAttributeType? Type { get; set; }
    }
}