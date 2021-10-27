using Microsoft.AspNetCore.Identity;

namespace eLearn.Modules.Users.Core.Entities
{
    internal class ELearnUser : IdentityUser
    {
        public string Locale { get; set; } = "en-GB";
    }
}