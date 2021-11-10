using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Shared.Infrastructure.Entities;

namespace eLearn.Modules.Users.Core.Entities
{
    internal class AppRoleClaim : IdentityRoleClaim<long>, IEntityBase
    {
        public string? Description { get; set; }
        public string? Group { get; set; }
        public virtual AppRole? Role { get; set; }

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
    }
}