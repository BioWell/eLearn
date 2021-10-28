using System.Collections.Generic;
using Shared.Infrastructure.Entities;

namespace eLearn.Modules.Users.Core.Entities
{
    internal class Address : EntityBase
    {
        public Address()
        {
        }

        public Address(long id)
        {
            Id = id;
        }

        public string ContactName { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string AddressLine1 { get; set; } = null!;
        public string AddressLine2 { get; set; } = null!;
        public string City { get; set; } = null!;
        public string ZipCode { get; set; } = null!;
        public long? DistrictId { get; set; }
        public District District { get; set; } = null!;
        public long StateOrProvinceId { get; set; }
        public StateOrProvince StateOrProvince { get; set; } = null!;
        public string CountryId { get; set; } = null!;
        public Country Country { get; set; } = null!;
        public IList<UserAddress> UserAddresses { get; set; } = new List<UserAddress>();
    }
}