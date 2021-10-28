using System;
using Shared.Infrastructure.Entities;

namespace eLearn.Modules.Users.Core.Entities
{
    internal class UserAddress : EntityBase
    {
        public long UserId { get; set; }

        public User User { get; set; } = null!;

        public long AddressId { get; set; }

        public Address Address { get; set; } = null!;

        public AddressType AddressType { get; set; }

        public DateTimeOffset? LastUsedOn { get; set; } = null!;
    }
}