using System;
using System.Security.Cryptography;
using System.Threading.Tasks;
using eLearn.Modules.Users.Core.Dto.Identity.Tokens;
using eLearn.Modules.Users.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using Shared.Infrastructure.Services.Email;
using Shared.Infrastructure.Wrapper;

namespace eLearn.Modules.Users.Core.Services
{
    internal class TokenService : ITokenService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IStringLocalizer<TokenService> _localizer;

        private readonly MailSettings _mailSettings;
        // private readonly JwtSettings _config;
        // private readonly IEventLogService _eventLog;

        public TokenService(UserManager<AppUser> userManager, 
            RoleManager<AppRole> roleManager,
            IStringLocalizer<TokenService> localizer, 
            MailSettings mailSettings)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _localizer = localizer;
            _mailSettings = mailSettings;
        }

        public Task<IResult<TokenResponse>> RefreshTokenAsync(RefreshTokenRequest request, string ipAddress)
        {
            throw new System.NotImplementedException();
        }
        
        public string GenerateRefreshToken()
        {
            byte[] randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}