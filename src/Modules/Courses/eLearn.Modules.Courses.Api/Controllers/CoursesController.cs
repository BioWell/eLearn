using Microsoft.AspNetCore.Mvc;

namespace eLearn.Modules.Courses.Api.Controllers
{
    internal class CoursesController: BaseController
    {
        [HttpGet]
        public ActionResult<string> Get() => Ok("Course module"); 
    }
}