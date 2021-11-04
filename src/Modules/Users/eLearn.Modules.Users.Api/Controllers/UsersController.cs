using Microsoft.AspNetCore.Mvc;

namespace eLearn.Modules.Users.Api.Controllers
{
    [Route(BasePath + "/[controller]")]
    internal sealed class UsersController: BaseController
    {
        [HttpGet]
        public ActionResult<string> Get() => Ok("Users Controllers");
    }
}