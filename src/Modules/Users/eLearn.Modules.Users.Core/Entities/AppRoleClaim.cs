using Microsoft.AspNetCore.Identity;

namespace eLearn.Modules.Users.Core.Entities
{
    internal class AppRoleClaim : IdentityRoleClaim<long>
    {
        public string? Description { get; set; }
        public string? Group { get; set; }
        public override long RoleId { get; set; }
        public virtual AppRole? Role { get; set; }
    }
}