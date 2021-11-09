using Shared.Infrastructure.Entities;

namespace eLearn.Modules.Users.Core.Entities
{
    internal class District : BaseEntity
    {
        public District()
        {
        }

        public District(long id) => Id = id;
        public string? Name { get; set; }
        public string? Type { get; set; }
        public string? Location { get; set; }
        public long? StateOrProvinceId { get; set; }
        public virtual StateOrProvince? StateOrProvince { get; set; }
    }
}