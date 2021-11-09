using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Shared.Infrastructure.Api.Contracts;
using Shared.Infrastructure.Entities;

namespace Shared.Infrastructure.CQRS
{
    public interface IExtendedAttributeDbContext<TEntityId, TEntity, TExtendedAttribute> //: IDbContext
        where TEntity : class, IEntity<TEntityId>
        where TExtendedAttribute : ExtendedAttribute<TEntityId, TEntity>
    {
        [NotMapped]
        public DbSet<TEntity> Entities => GetEntities();

        protected DbSet<TEntity> GetEntities();

        public DbSet<TExtendedAttribute> ExtendedAttributes { get; set; }
    }
}