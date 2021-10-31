using Microsoft.AspNetCore.Mvc;
using Shared.Infrastructure.Api;

namespace eLearn.Modules.Courses.Api.Controllers
{
    [ApiController]
    [Route(BasePath + "/[controller]")]
    [ProducesDefaultContentType]
    internal abstract class BaseController : CommonBaseController
    {
        protected internal new const string BasePath = CommonBaseController.BasePath + "/courses";
        protected ActionResult<T> OkOrNotFound<T>(T model)
        {
            if (model is not null)
            {
                return Ok(model);
            }

            return NotFound();
        }
    }
}