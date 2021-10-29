using System.Collections.Generic;
using Shared.Infrastructure.Entities;

namespace eLearn.Modules.Users.Core.Entities
{
    internal class Address : EntityBase
    {
        public Address() { }
        public Address(long id) => Id = id;
        public string ContactName { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string AddressLine1 { get; set; } = null!;
        public string AddressLine2 { get; set; } = null!;
        public string City { get; set; } = null!;
        public string ZipCode { get; set; } = null!;
        
        public long? DistrictId { get; set; }
        public virtual District District { get; set; } = new District();
        public long StateOrProvinceId { get; set; }
        public virtual StateOrProvince StateOrProvince { get; set; } = new StateOrProvince();
        public string CountryId { get; set; } = null!;
        public virtual Country Country { get; set; } = new Country();
        
        public IList<UserAddress> UserAddresses { get; set; } = new List<UserAddress>();
    }
}