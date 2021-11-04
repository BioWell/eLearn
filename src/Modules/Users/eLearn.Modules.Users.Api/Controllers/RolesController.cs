using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Infrastructure.Auth;

namespace eLearn.Modules.Users.Api.Controllers
{
    [Microsoft.AspNetCore.Components.Route(BasePath)]
    internal sealed class RolesController : BaseController
    {
        [HttpGet]
        [Authorize(Policy = Permissions.Roles.View)]
        public async Task<IActionResult> GetAllAsync()
        {
            // var roles = await _roleService.GetAllAsync();
            // return Ok(roles);
            return Ok("roles");
        }
    }
}