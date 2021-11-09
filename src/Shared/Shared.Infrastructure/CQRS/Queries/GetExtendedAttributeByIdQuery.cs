using System;
using MediatR;
using Shared.Infrastructure.Caching;
using Shared.Infrastructure.Entities;
using Shared.Infrastructure.Wrapper;

namespace Shared.Infrastructure.CQRS.Queries
{
    public class
        GetExtendedAttributeByIdQuery<TEntityId, TEntity> :
            IRequest<Result<GetExtendedAttributeByIdResponse<TEntityId>>>, 
            ICacheable
        where TEntity : class, IEntity<TEntityId>
    {
        public long Id { get; protected set; }

        public bool BypassCache { get; protected set; }

        public string CacheKey { get; protected set; } = String.Empty;

        public TimeSpan? SlidingExpiration { get; protected set; }
    }
}