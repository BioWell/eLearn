namespace Shared.Infrastructure.Entities
{
    public abstract class EntityBaseWithTypedId<TId> : ValidatableObject, IEntity<TId>
    {
        public virtual TId? Id { get; protected set; } 
    }
}