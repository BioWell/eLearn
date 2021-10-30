using System.Collections.Generic;
using Shared.Infrastructure.Entities;

namespace eLearn.Modules.Users.Core.Entities
{
    internal class Country : EntityBaseWithTypedId<string>
    {
        public Country()
        {
        }

        public Country(string id) => Id = id;
        public string? Name { get; set; }
        public string? Code3 { get; set; }
        public bool IsBillingEnabled { get; set; }
        public bool IsShippingEnabled { get; set; }
        public bool IsCityEnabled { get; set; } = true;
        public bool IsZipCodeEnabled { get; set; } = true;
        public bool IsDistrictEnabled { get; set; } = true;
        public long StateOrProvinceId { get; set; }
        public virtual IList<StateOrProvince> StatesOrProvinces { get; set; } = new List<StateOrProvince>();
    }
}