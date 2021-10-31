using Microsoft.AspNetCore.Mvc;

namespace eLearn.Modules.Users.Api.Controllers
{
    [Route(BasePath)]
    internal sealed class UsersController: BaseController
    {
        [HttpGet]
        public ActionResult<string> Get() => Ok("Users module");
    }
}