using System.Collections.Generic;
using Shared.Infrastructure.Entities;

namespace eLearn.Modules.Users.Core.Entities
{
    internal class Address : EntityBase
    {
        public Address()
        {
        }

        public Address(long id) => Id = id;
        public string ContactName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string AddressLine1 { get; set; } = string.Empty;
        public string AddressLine2 { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string ZipCode { get; set; } = string.Empty;
        public long? DistrictId { get; set; }
        public virtual District District { get; set; } = new District();
        public long StateOrProvinceId { get; set; }
        public virtual StateOrProvince StateOrProvince { get; set; } = new StateOrProvince();
        public string CountryId { get; set; } = string.Empty;
        public virtual Country Country { get; set; } = new Country();
        public virtual IList<UserAddress> UserAddresses { get; set; } = new List<UserAddress>();
    }
}