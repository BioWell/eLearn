using System.Threading.Tasks;
using eLearn.Modules.Users.Core.Commands;
using eLearn.Modules.Users.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eLearn.Modules.Users.Api.Controllers
{
    [Route(BasePath)]
    internal sealed class IdentityController : BaseController
    {
        private readonly IIdentityService _identityService;

        public IdentityController(IIdentityService identityService)
        {
            _identityService = identityService;
        }
        
        [HttpPost("register")]
        //[AllowAnonymous]
        public async Task<ActionResult> SignUpAsync(RegisterRequest request)
        {
            var origin = Request.Headers["origin"];
            return Ok(await _identityService.RegisterAsync(request, origin));
        }
        
        [HttpGet("confirm-email")]
        // [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmailAsync([FromQuery] string userId, [FromQuery] string code)
        {
            return Ok(await _identityService.ConfirmEmailAsync(userId, code));
        }
        
        // [HttpGet("confirm-phone-number")]
        // // [AllowAnonymous]
        // public async Task<IActionResult> ConfirmPhoneNumberAsync([FromQuery] string userId, [FromQuery] string code)
        // {
        //     return Ok(await _identityService.ConfirmPhoneNumberAsync(userId, code));
        // }
        //
        // [HttpPost("sign-in")]
        // [ProducesResponseType(StatusCodes.Status204NoContent)]
        // [ProducesResponseType(StatusCodes.Status400BadRequest)]
        // public async Task<ActionResult<UserDetailsDto>> SignInAsync(SignIn command)
        // {
        //     // await _dispatcher.SendAsync(command.Bind(x => x.Id, Guid.NewGuid()));
        //     // var jwt = _userRequestStorage.GetToken(command.Id);
        //     // var appUser = await _dispatcher.QueryAsync(new GetUser {UserId = jwt.UserId});
        //     // AddCookie(AccessTokenCookie, jwt.AccessToken);
        //     return Ok(appUser);
        // }
    }
}