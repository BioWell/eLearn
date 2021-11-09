﻿using System;
using Shared.Infrastructure.Api.Contracts;

namespace Shared.Infrastructure.Entities
{
    public abstract class ExtendedAttribute<TEntityId, TEntity>
        : EntityBase, IExtendedAttribute<TEntityId>
        where TEntity : class, IEntity<TEntityId>
    {
        public TEntityId EntityId { get; set; }

        public virtual TEntity Entity { get; set; }

        public ExtendedAttributeType Type { get; set; }

        public string Key { get; set; }

        public decimal? Decimal { get; set; }

        public string? Text { get; set; }

        public DateTime? DateTime { get; set; }

        public string? Json { get; set; }

        public bool? Boolean { get; set; }

        public int? Integer { get; set; }

        public string? ExternalId { get; set; }

        public string? Group { get; set; }

        public string? Description { get; set; }

        public bool IsActive { get; set; }
    }
}