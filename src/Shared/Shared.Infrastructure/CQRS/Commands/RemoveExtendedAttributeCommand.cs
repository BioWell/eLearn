using MediatR;
using Shared.Infrastructure.Entities;
using Shared.Infrastructure.Wrapper;

namespace Shared.Infrastructure.CQRS.Commands
{
    public class RemoveExtendedAttributeCommand<TEntityId, TEntity> : IRequest<Result<long>>
        where TEntity : class, IEntity<TEntityId>
    {
        public long Id { get; }

        public RemoveExtendedAttributeCommand(long entityExtendedAttributeId)
        {
            Id = entityExtendedAttributeId;
        }
    }
}