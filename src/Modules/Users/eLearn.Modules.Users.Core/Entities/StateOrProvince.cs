using Shared.Infrastructure.Entities;

namespace eLearn.Modules.Users.Core.Entities
{
    internal class StateOrProvince : EntityBase
    {
        public StateOrProvince()
        {
        }

        public StateOrProvince(long id) => Id = id;
        public string? Code { get; set; }
        public string Name { get; set; } = "Washington";
        public string? Type { get; set; }
        public string CountryId { get; set; } = "US";
        public virtual Country Country { get; set; } = new Country();
    }
}