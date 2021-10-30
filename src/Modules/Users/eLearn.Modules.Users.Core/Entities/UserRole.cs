using Microsoft.AspNetCore.Identity;

namespace eLearn.Modules.Users.Core.Entities
{
    internal class UserRole : IdentityUserRole<long>
    {
        public override long UserId { get; set; }
        public virtual User User { get; set; } = new User();
        public override long RoleId { get; set; }
        public virtual Role Role { get; set; } = new Role();
    }
}