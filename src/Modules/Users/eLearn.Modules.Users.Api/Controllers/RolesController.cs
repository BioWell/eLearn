using System.Threading.Tasks;
using eLearn.Modules.Users.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Infrastructure.Auth;

namespace eLearn.Modules.Users.Api.Controllers
{
    [Route(BasePath + "/[controller]")]
    internal sealed class RolesController : BaseController
    {
        private readonly IRoleService _roleService;

        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }
        //
        // [HttpGet]   
        // public ActionResult<string> Get() => Ok("Roles Controller");
        
        [HttpGet]
        // [Authorize(Policy = Permissions.Roles.View)]
        public async Task<IActionResult> GetAllAsync()
        {
            var roles = await _roleService.GetAllAsync();
            return Ok(roles);
        }
    }
}