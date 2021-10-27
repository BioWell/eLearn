using Microsoft.AspNetCore.Mvc;
using Shared.Infrastructure.Api;

namespace eLearn.Modules.Courses.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [ProducesDefaultContentType]
    internal abstract class BaseController : ControllerBase
    {
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