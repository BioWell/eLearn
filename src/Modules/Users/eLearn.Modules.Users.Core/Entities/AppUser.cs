using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Shared.Infrastructure.Entities;

namespace eLearn.Modules.Users.Core.Entities
{
    internal class AppUser : IdentityUser<long>, IEntity<long>, IExtendableObject, IEntityBase
    {
        public AppUser()
        {
            CreatedOn = DateTimeOffset.Now;
            LatestUpdatedOn = DateTimeOffset.Now;
        }

        public Guid UserGuid { get; set; } = Guid.NewGuid();
        public string FirstName { get; set; } = String.Empty;
        public string LastName { get; set; } = String.Empty;
        public bool IsActive { get; set; }
        public string ImageUrl { get; set; } = String.Empty;
        public string? RefreshTokenHash { get; set; }
        public string? Culture { get; set; }
        public string ExtensionData { get; set; } = String.Empty;
        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset LatestUpdatedOn { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }

        public virtual IList<AppUserRole>? UserRoles { get; set; }
        public virtual IList<UserAddress>? Addresses { get; set; }


        private List<Event> _domainEvents;

        public IReadOnlyCollection<Event> DomainEvents => _domainEvents?.AsReadOnly();

        public void AddDomainEvent(Event domainEvent)
        {
            _domainEvents ??= new List<Event>();
            _domainEvents.Add(domainEvent);
        }

        public void RemoveDomainEvent(Event domainEvent)
        {
            _domainEvents?.Remove(domainEvent);
        }

        public void ClearDomainEvents()
        {
            _domainEvents?.Clear();
        }

        // public long? DefaultShippingAddressId { get; set; }
        // public virtual UserAddress? DefaultShippingAddress { get; set; }
        // public long? DefaultBillingAddressId { get; set; }
        // public virtual UserAddress? DefaultBillingAddress { get; set; }
    }
}