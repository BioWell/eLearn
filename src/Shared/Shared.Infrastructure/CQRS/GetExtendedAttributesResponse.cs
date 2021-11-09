using System;
using Shared.Infrastructure.Entities;

namespace Shared.Infrastructure.CQRS
{
    public record GetExtendedAttributesResponse<TEntityId>(
        long Id,
        TEntityId EntityId,
        ExtendedAttributeType Type,
        string Key,
        decimal? Decimal,
        string? Text,
        DateTime? DateTime,
        string? Json,
        bool? Boolean,
        int? Integer,
        string? ExternalId,
        string? Group,
        string? Description,
        bool IsActive);
}