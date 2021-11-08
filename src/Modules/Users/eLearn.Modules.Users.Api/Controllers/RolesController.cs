using System.Threading.Tasks;
using eLearn.Modules.Users.Core.Commands;
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
        private readonly IRoleClaimService _roleClaimService;
        
        public RolesController(IRoleService roleService, 
            IRoleClaimService roleClaimService)
        {
            _roleService = roleService;
            _roleClaimService = roleClaimService;
        }
        
        [HttpGet]
        [Authorize(Policy = Permissions.Roles.View)]
        public async Task<IActionResult> GetAllAsync()
        {
            var roles = await _roleService.GetAllAsync();
            return Ok(roles);
        }
        
        [HttpPost]
        [Authorize(Policy = Permissions.Roles.Create)]
        public async Task<IActionResult> PostAsync(RoleRequest request)
        {
            var response = await _roleService.SaveAsync(request);
            return Ok(response);
        }
        
        [HttpDelete("{id}")]
        [Authorize(Policy = Permissions.Roles.Delete)]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            var response = await _roleService.DeleteAsync(id);
            return Ok(response);
        }
        
        [HttpGet("permissions")]
        [Authorize(Policy = Permissions.RoleClaims.View)]
        public async Task<IActionResult> GetAllClaimsAsync()
        {
            var response = await _roleClaimService.GetAllAsync();
            return Ok(response);
        }
        
        [HttpGet("permissions/byrole/{roleId}")]
        [Authorize(Policy = Permissions.RoleClaims.View)]
        public async Task<IActionResult> GetPermissionsByRoleIdAsync([FromRoute] string roleId)
        {
            //Get Permissions By Role Id.
            var response = await _roleClaimService.GetAllPermissionsAsync(roleId);
            return Ok(response);
        }
        
        [HttpGet("permissions/{id}")]
        [Authorize(Policy = Permissions.RoleClaims.View)]
        public async Task<IActionResult> GetClaimByIdAsync([FromRoute] int id)
        {
            //Get a Role Claim By Id.
            var response = await _roleClaimService.GetByIdAsync(id);
            return Ok(response);
        }
        
        [HttpPut("permissions/update")]
        [Authorize(Policy = Permissions.RoleClaims.Edit)]
        public async Task<IActionResult> UpdatePermissionsAsync(PermissionRequest request)
        {
            //Edit a Role Claims.
            var response = await _roleClaimService.UpdatePermissionsAsync(request);
            return Ok(response);
        }
        
        [HttpDelete("permissions/{id}")]
        [Authorize(Policy = Permissions.RoleClaims.Delete)]
        public async Task<IActionResult> DeleteClaimByIdAsync([FromRoute] int id)
        {
            //Delete a Role Claim By Id.
            var response = await _roleClaimService.DeleteAsync(id);
            return Ok(response);
        }
    }
}