using System.Threading.Tasks;
using eLearn.Modules.Users.Core.Dto.Identity.Tokens;
using eLearn.Modules.Users.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eLearn.Modules.Users.Api.Controllers
{
    [Route(BasePath)]
    internal sealed class TokensController : BaseController
    {
        private readonly ITokenService _tokenService;

        public TokensController(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }
        
        [HttpPost("refresh")]
        [AllowAnonymous]
        public async Task<ActionResult> RefreshAsync(RefreshTokenRequest request)
        {
            var response = await _tokenService.RefreshTokenAsync(request, GenerateIpAddress());
            return Ok(response);
        }
    }
}