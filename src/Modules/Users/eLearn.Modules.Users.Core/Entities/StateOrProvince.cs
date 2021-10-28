using Shared.Infrastructure.Entities;

namespace eLearn.Modules.Users.Core.Entities
{
    internal class StateOrProvince : EntityBase
    {
        public StateOrProvince()
        {
        }

        public StateOrProvince(long id)
        {
            Id = id;
        }

        public string CountryId { get; set; } = null!;
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Type { get; set; } = null!;
    }
}