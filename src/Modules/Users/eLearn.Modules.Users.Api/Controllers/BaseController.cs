using Microsoft.AspNetCore.Mvc;
using Shared.Infrastructure.Api;

namespace eLearn.Modules.Users.Api.Controllers
{
    [ApiController]
    [Route(BasePath + "/[controller]")]
    [ProducesDefaultContentType]
    internal abstract class BaseController : CommonBaseController
    {
        protected internal new const string BasePath = CommonBaseController.BasePath + "/users";
    }
}