using System;
using Shared.Infrastructure.Entities;

namespace eLearn.Modules.Users.Core.Entities
{
    internal class UserAddress : EntityBase
    {
        public AddressType AddressType { get; set; }
        public DateTimeOffset? LastUsedOn { get; set; }
        public long UserId { get; set; }
        public virtual AppUser? User { get; set; }
        public long AddressId { get; set; }
        public virtual Address? Address { get; set; }
    }
}