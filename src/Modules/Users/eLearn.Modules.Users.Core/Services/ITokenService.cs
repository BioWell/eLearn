using System.Threading.Tasks;
using eLearn.Modules.Users.Core.Dto.Identity.Tokens;
using eLearn.Modules.Users.Core.Entities;
using Shared.Infrastructure.Wrapper;

namespace eLearn.Modules.Users.Core.Services
{
    internal interface ITokenService
    {
        string GenerateRefreshToken();
        Task<string> GenerateJwtAsync(AppUser user, string? ipAddress);
        Task<IResult<TokenResponse>> RefreshTokenAsync(RefreshTokenRequest request, string ipAddress);
    }
}