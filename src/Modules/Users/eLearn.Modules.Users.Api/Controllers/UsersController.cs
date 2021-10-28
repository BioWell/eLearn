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
        [HttpGet]
        public ActionResult<string> Get() => Ok("Users module");
    }
}