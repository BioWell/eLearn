using System;

namespace Shared.Infrastructure.Auth
{
    public class AuthSettings
    {
        public string Key { get; set; } = String.Empty;

        public int TokenExpirationInMinutes { get; set; }

        public int RefreshTokenExpirationInDays { get; set; }       
    }
}