using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Shared.Infrastructure.Entities;

namespace eLearn.Modules.Users.Core.Entities
{
    internal class AppRole : IdentityRole<long>, IEntity<long> ,IEntityBase
    {
        public string Description { get; set; } = String.Empty;
        public virtual IList<AppUserRole>? UserRoles { get; set; }
        public virtual IList<AppRoleClaim>? RoleClaims { get; set; }
        
        public AppRole()
            : base()
        {
        }
        
        public AppRole(string roleName, string? roleDescription)
            : base(roleName)
        {
            Description = roleDescription?? String.Empty;
        }
        
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