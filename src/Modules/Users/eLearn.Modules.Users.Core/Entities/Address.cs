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
        public string? ContactName { get; set; }
        public string? Phone { get; set; }
        public string? AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        public string? City { get; set; }
        public string? ZipCode { get; set; }

        public long? DistrictId { get; set; }
        public virtual District? District { get; set; }
        public long StateOrProvinceId { get; set; }
        public virtual StateOrProvince? StateOrProvince { get; set; }
        public string? CountryId { get; set; }
        public virtual Country? Country { get; set; }
        public virtual IList<UserAddress>? Users { get; set; }
    }
}