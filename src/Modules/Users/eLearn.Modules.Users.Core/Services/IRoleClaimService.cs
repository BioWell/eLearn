using System.Collections.Generic;
using System.Threading.Tasks;
using eLearn.Modules.Users.Core.Commands;
using eLearn.Modules.Users.Core.Dto.Identity.Roles;
using Shared.Infrastructure.Wrapper;

namespace eLearn.Modules.Users.Core.Services
{
    internal interface IRoleClaimService
    {
        Task<Result<List<RoleClaimResponse>>> GetAllAsync();

        Task<int> GetCountAsync();
        
        Task<Result<RoleClaimResponse>> GetByIdAsync(int id);
        
        Task<Result<List<RoleClaimResponse>>> GetAllByRoleIdAsync(string roleId);
        
        Task<Result<string>> SaveAsync(RoleClaimRequest request);
        
        Task<Result<string>> DeleteAsync(int id);
        
        Task<Result<PermissionResponse>> GetAllPermissionsAsync(string roleId);
        
        Task<Result<string>> UpdatePermissionsAsync(PermissionRequest request);
    }
    
}