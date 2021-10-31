using System.Threading.Tasks;
using eLearn.Modules.Users.Core.Commands;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eLearn.Modules.Users.Api.Controllers
{
    [Route(BasePath)]
    internal class AccountController : BaseController
    {
        private readonly IMediator _mediator;

        public AccountController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("sign-up")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> SignUpAsync(SignUp command)
        {
            await _mediator.Send(command);
            return NoContent();
        }
        
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