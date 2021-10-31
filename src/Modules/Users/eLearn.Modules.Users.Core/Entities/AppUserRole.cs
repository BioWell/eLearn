using Microsoft.AspNetCore.Identity;

namespace eLearn.Modules.Users.Core.Entities
{
    internal sealed class AppUserRole : IdentityUserRole<long>
    {
        public AppUser? User { get; set; } 
        public AppRole? Role { get; set; }
    }
}