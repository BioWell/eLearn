using MediatR;
using Shared.Infrastructure.Api.Filters;
using Shared.Infrastructure.Entities;
using Shared.Infrastructure.Wrapper;

namespace Shared.Infrastructure.CQRS.Queries
{
    public class GetExtendedAttributesQuery<TEntityId, TEntity> : IRequest<PaginatedResult<GetExtendedAttributesResponse<TEntityId>>>
        where TEntity : class, IEntity<TEntityId>
    {
        public int PageNumber { get; }

        public int PageSize { get; }

        public string? SearchString { get; }

        public string[] OrderBy { get; }

        public TEntityId? EntityId { get; }

        public ExtendedAttributeType? Type { get; }

        public GetExtendedAttributesQuery(PaginatedExtendedAttributeFilter<TEntityId, TEntity> filter)
        {
            PageNumber = filter.PageNumber;
            PageSize = filter.PageSize;
            SearchString = filter.SearchString;
            OrderBy = new OrderByConverter().Convert(filter.OrderBy, null);
            EntityId = filter.EntityId;
            Type = filter.Type;
        }
    }
}