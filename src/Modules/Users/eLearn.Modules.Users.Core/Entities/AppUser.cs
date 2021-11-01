using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Shared.Infrastructure.Entities;

namespace eLearn.Modules.Users.Core.Entities
{
    internal class AppUser : IdentityUser<long>, IEntityWithTypedId<long>, IExtendableObject
    {
        public AppUser()
        {
            CreatedOn = DateTimeOffset.Now;
            LatestUpdatedOn = DateTimeOffset.Now;
        }

        public const string SettingsDataKey = "Settings";

        public Guid UserGuid { get; set; } 
        public string FullName { get; set; } = string.Empty;
        public string? RefreshTokenHash { get; set; }
        public string? Culture { get; set; }
        public string ExtensionData { get; set; } = string.Empty;
        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset LatestUpdatedOn { get; set; }
        public long? VendorId { get; set; }
        public bool IsDeleted { get; set; }

        public string ImageUrl { get; set; } = string.Empty;
        
        public virtual IList<AppUserRole>? UserRoles { get; set; }
        
        public virtual IList<UserAddress>? Addresses { get; set; }
        // public long? DefaultShippingAddressId { get; set; }
        // public virtual UserAddress? DefaultShippingAddress { get; set; }
        // public long? DefaultBillingAddressId { get; set; }
        // public virtual UserAddress? DefaultBillingAddress { get; set; }
    }
}