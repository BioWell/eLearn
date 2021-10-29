namespace Shared.Infrastructure.Entities
{
#pragma warning disable CS8618
    public abstract class EntityBaseWithTypedId<TId> : ValidatableObject, IEntityWithTypedId<TId>
    {
        public virtual TId Id { get; protected set; } 
    }
#pragma warning restore CS8618
}