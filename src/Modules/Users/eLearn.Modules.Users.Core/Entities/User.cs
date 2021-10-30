using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Shared.Infrastructure.Entities;

namespace eLearn.Modules.Users.Core.Entities
{
    internal class User : IdentityUser<long>, IEntityWithTypedId<long>, IExtendableObject
    {
        public User()
        {
            CreatedOn = DateTimeOffset.Now;
            LatestUpdatedOn = DateTimeOffset.Now;
        }

        public const string SettingsDataKey = "Settings";
        public string FullName { get; set; } = string.Empty;
        public string? RefreshTokenHash { get; set; }
        public string? Culture { get; set; }
        public string ExtensionData { get; set; } = string.Empty;
        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset LatestUpdatedOn { get; set; }
        public long? VendorId { get; set; }
        public bool IsDeleted { get; set; }

        public virtual IList<UserRole> Roles { get; set; } = new List<UserRole>();
        public virtual IList<UserAddress> Addresses { get; set; } = new List<UserAddress>();

        public long? DefaultShippingAddressId { get; set; }
        public virtual UserAddress DefaultShippingAddress { get; set; } = new UserAddress();
        
        public long? DefaultBillingAddressId { get; set; }
        public virtual UserAddress DefaultBillingAddress { get; set; } = new UserAddress();
    }
}