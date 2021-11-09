namespace Shared.Infrastructure.Api.Contracts
{
    public interface IEntity<TEntityId> : IEntity
    {
        public TEntityId Id { get; set; }
    }

    public interface IEntity
    {
    }
}