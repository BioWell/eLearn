using System.Collections.Generic;

namespace eLearn.Modules.Users.Core
{
    public class RegistrationOptions
    {
        public bool Enabled { get; set; }
        public IEnumerable<string> InvalidEmailProviders { get; set; } = System.Array.Empty<string>();
    }
}