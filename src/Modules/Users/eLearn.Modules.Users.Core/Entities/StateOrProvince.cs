using System.Collections.Generic;
using Shared.Infrastructure.Entities;

namespace eLearn.Modules.Users.Core.Entities
{
    internal class StateOrProvince : BaseEntity
    {
        public StateOrProvince()
        {
        }

        public StateOrProvince(long id) => Id = id;
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? Type { get; set; }
        public string? CountryId { get; set; }
        public virtual Country? Country { get; set; }
    }
}