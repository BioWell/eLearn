using Microsoft.AspNetCore.Mvc;
using Shared.Infrastructure.Api;

namespace eLearn.Modules.Users.Api.Controllers
{
    [ApiController]
    [Route(BasePath + "/[controller]")]
    [ProducesDefaultContentType]
    internal abstract class BaseController : CommonBaseController
    {
        protected internal new const string BasePath = CommonBaseController.BasePath + "/identity";
        
        protected string? GenerateIpAddress()
        {
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
            {
                return Request.Headers["X-Forwarded-For"];
            }
            else
            {
                return HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString();
            }
        }
    }
}