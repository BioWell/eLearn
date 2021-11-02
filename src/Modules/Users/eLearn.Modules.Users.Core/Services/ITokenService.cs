using System.Threading.Tasks;
using eLearn.Modules.Users.Core.Dto.Identity.Tokens;
using Shared.Infrastructure.Wrapper;

namespace eLearn.Modules.Users.Core.Services
{
    internal interface ITokenService
    {
        string GenerateRefreshToken();
        Task<IResult<TokenResponse>> RefreshTokenAsync(RefreshTokenRequest request, string ipAddress);
    }
}