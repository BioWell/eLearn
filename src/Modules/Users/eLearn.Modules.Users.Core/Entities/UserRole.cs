using Microsoft.AspNetCore.Identity;

namespace eLearn.Modules.Users.Core.Entities
{
    internal class UserRole : IdentityUserRole<long>
    {
        public override long UserId { get; set; }

        public User User { get; set; } = null!;

        public override long RoleId { get; set; }

        public Role Role { get; set; } = null!;
    }
}