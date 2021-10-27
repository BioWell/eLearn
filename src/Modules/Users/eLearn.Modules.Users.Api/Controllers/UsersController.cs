using Microsoft.AspNetCore.Mvc;
using Shared.Infrastructure.Api;

namespace eLearn.Modules.Users.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [ProducesDefaultContentType]
    internal class UsersController: ControllerBase
    {
        [HttpGet]
        public ActionResult<string> Get() => Ok("Users module"); 
    }
}