using Shared.Infrastructure.Entities;

namespace eLearn.Modules.Users.Core.Entities
{
    internal class StateOrProvince : EntityBase
    {
        public StateOrProvince()
        {
        }

        public StateOrProvince(long id) => Id = id;
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string CountryId { get; set; } = string.Empty;
        public virtual Country Country { get; set; } = new Country();
    }
}