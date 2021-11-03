using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using eLearn.Modules.Users.Core.Dto.Identity.Tokens;
using eLearn.Modules.Users.Core.Entities;
using eLearn.Modules.Users.Core.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using Microsoft.IdentityModel.Tokens;
using Shared.Infrastructure.Auth;
using Shared.Infrastructure.Wrapper;

namespace eLearn.Modules.Users.Core.Services
{
    internal class TokenService : ITokenService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly AuthSettings _authSettings;
        private readonly IStringLocalizer<TokenService> _localizer;
        // private readonly IEventLogService _eventLog;

        public TokenService(UserManager<AppUser> userManager,
            RoleManager<AppRole> roleManager, 
            AuthSettings authSettings, 
            IStringLocalizer<TokenService> localizer)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _authSettings = authSettings;
            _localizer = localizer;
        }

        public async Task<IResult<TokenResponse>> RefreshTokenAsync(RefreshTokenRequest request, string ipAddress)
        {
            if (request is null)
            {
                throw new IdentityException(_localizer["Invalid Client Token."], statusCode: HttpStatusCode.Unauthorized);
            }

            var userPrincipal = GetPrincipalFromExpiredToken(request.Token);
            string userEmail = userPrincipal.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(userEmail);
            if (user == null)
            {
                throw new IdentityException(_localizer["User Not Found."], statusCode: HttpStatusCode.NotFound);
            }

            if (user.RefreshToken != request.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
            {
                throw new IdentityException(_localizer["Invalid Client Token."], statusCode: HttpStatusCode.Unauthorized);
            }

            string token = GenerateEncryptedToken(GetSigningCredentials(), await GetClaimsAsync(user, ipAddress));
            user.RefreshToken = GenerateRefreshToken();
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_authSettings.RefreshTokenExpirationInDays);
            await _userManager.UpdateAsync(user);
            var response = new TokenResponse(token, user.RefreshToken, user.RefreshTokenExpiryTime);
            return await Result<TokenResponse>.SuccessAsync(response);
        }

        public string GenerateRefreshToken()
        {
            byte[] randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        public async Task<string> GenerateJwtAsync(AppUser user, string? ipAddress)
        {
            return GenerateEncryptedToken(GetSigningCredentials(), await GetClaimsAsync(user, ipAddress));
        }
        
        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authSettings.Key)),
                ValidateIssuer = false,
                ValidateAudience = false,
                RoleClaimType = ClaimTypes.Role,
                ClockSkew = TimeSpan.Zero
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken ||
                !jwtSecurityToken.Header.Alg.Equals(
                    SecurityAlgorithms.HmacSha256,
                    StringComparison.InvariantCultureIgnoreCase))
            {
                throw new IdentityException(_localizer["Invalid Token."], statusCode: HttpStatusCode.Unauthorized);
            }

            return principal;
        }

        private SigningCredentials GetSigningCredentials()
        {
            byte[] secret = Encoding.UTF8.GetBytes(_authSettings.Key);
            return new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256);
        }

        private string GenerateEncryptedToken(SigningCredentials signingCredentials, IEnumerable<Claim> claims)
        {
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_authSettings.TokenExpirationInMinutes),
                signingCredentials: signingCredentials);
            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }

        private async Task<IEnumerable<Claim>> GetClaimsAsync(AppUser user, string? ipAddress)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();
            var permissionClaims = new List<Claim>();
            foreach (string role in roles)
            {
                roleClaims.Add(new Claim(ClaimTypes.Role, role));
                var thisRole = await _roleManager.FindByNameAsync(role);
                var allPermissionsForThisRoles = await _roleManager.GetClaimsAsync(thisRole);
                permissionClaims.AddRange(allPermissionsForThisRoles);
            }

            return new List<Claim>
                {
                    new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new(ClaimTypes.Email, user.Email),
                    new("fullName", $"{user.FirstName} {user.LastName}"),
                    new(ClaimTypes.Name, user.FirstName),
                    new(ClaimTypes.Surname, user.LastName),
                    new("ipAddress", ipAddress??String.Empty)
                }
                .Union(userClaims)
                .Union(roleClaims)
                .Union(permissionClaims);
        }
    }
}