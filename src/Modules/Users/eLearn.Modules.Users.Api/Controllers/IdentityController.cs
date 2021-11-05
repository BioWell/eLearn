using System.Threading.Tasks;
using eLearn.Modules.Users.Core.Commands;
using eLearn.Modules.Users.Core.Dto.Identity.Tokens;
using eLearn.Modules.Users.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Infrastructure.Wrapper;

namespace eLearn.Modules.Users.Api.Controllers
{
    [Route(BasePath + "/[controller]")]
    internal sealed class IdentityController : BaseController
    {
        private readonly IIdentityService _identityService;
        private readonly ITokenService _tokenService;
        public IdentityController(IIdentityService identityService, 
            ITokenService tokenService)
        {
            _identityService = identityService;
            _tokenService = tokenService;
        }
        
        [HttpGet]
        public ActionResult<string> Get() => Ok(Result<string>.SuccessAsync("Users module")); 
        
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult> RegisterAsync(RegisterRequest request)
        {
            var origin = Request.Headers["origin"];
            return Ok(await _identityService.RegisterAsync(request, origin));
        }
        
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult> LoginAsync(LoginRequest request)
        {
            var token = await _identityService.LoginAsync(request, GenerateIpAddress());
            return Ok(token);
        }
        
        [HttpGet("confirm-email")]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmailAsync([FromQuery] string userId, [FromQuery] string code)
        {
            return Ok(await _identityService.ConfirmEmailAsync(userId, code));
        }

        [HttpPost("forgot-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPasswordAsync(ForgotPasswordRequest request)
        {
            var origin = Request.Headers["origin"];
            return Ok(await _identityService.ForgotPasswordAsync(request, origin));
        }

        [HttpPost("reset-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPasswordAsync(ResetPasswordRequest request)
        {
            
            return Ok(await _identityService.ResetPasswordAsync(request));
        }
        
        [HttpPost("refresh-token")]
        [AllowAnonymous]
        public async Task<ActionResult> RefreshTokenAsync(RefreshTokenRequest request)
        {
            var response = await _tokenService.RefreshTokenAsync(request, GenerateIpAddress());
            return Ok(response);
        }
        
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordRequest request)
        {
            return Ok(await _identityService.ChangePasswordAsync(request));
        }
        
        private string? GenerateIpAddress()
        {
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
            {
                return Request.Headers["X-Forwarded-For"];
            }
            else
            {
                return HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString();
            }
        }
    }
}