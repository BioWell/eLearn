using System.Collections.Generic;

namespace eLearn.Modules.Users.Core
{
    public class RegistrationSettings
    {
        public bool Enabled { get; set; }
        public IEnumerable<string> InvalidEmailProviders { get; set; } = System.Array.Empty<string>();
    }
}