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
        
        public string FullName { get; set; } = null!;
        public long? VendorId { get; set; }
        public bool IsDeleted { get; set; }
        public DateTimeOffset CreatedOn { get; set; } 
        public DateTimeOffset LatestUpdatedOn { get; set; } 
        public IList<UserAddress> UserAddresses { get; set; } = new List<UserAddress>();
        public UserAddress DefaultShippingAddress { get; set; } = null!;
        public long? DefaultShippingAddressId { get; set; }
        public UserAddress DefaultBillingAddress { get; set; } = null!;
        public long? DefaultBillingAddressId { get; set; }
        public string RefreshTokenHash { get; set; } = null!;
        public IList<UserRole> Roles { get; set; } = new List<UserRole>();
        public string Culture { get; set; } = null!;
        public string ExtensionData { get; set; } = null!;
    }
}