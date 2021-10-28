namespace Shared.Infrastructure.Entities
{
    public interface IEntityWithTypedId<TId>
    {
        TId Id { get; }
    }
}