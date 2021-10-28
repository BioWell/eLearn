using Shared.Infrastructure.Entities;

namespace eLearn.Modules.Users.Core.Entities
{
    internal class District : EntityBase
    {
        public District()
        {
        }

        public District(long id)
        {
            Id = id;
        }

        public long StateOrProvinceId { get; set; }

        public StateOrProvince StateOrProvince { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string Type { get; set; } = null!;

        public string Location { get; set; } = null!;
    }
}