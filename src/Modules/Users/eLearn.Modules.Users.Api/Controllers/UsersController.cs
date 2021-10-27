using System.Threading.Tasks;
using eLearn.Modules.Users.Core.Commands;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Infrastructure.Api;

namespace eLearn.Modules.Users.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [ProducesDefaultContentType]
    internal class UsersController: ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
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
        
        [HttpGet]
        public ActionResult<string> Get() => Ok("Users module"); 
        
        // public async Task<IActionResult> GetAllAsync()
        // {
        //     var brands = await _mediator.Send(new GetAllBrandsQuery());
        //     return Ok(brands);
        // }
        //
        // [HttpPost]
        // public async Task<IActionResult> RegisterAsync(RegisterBrandCommand command)
        // {
        //     return Ok(await _mediator.Send(command));
        // }
    }
}