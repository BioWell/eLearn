using System.Collections.Generic;
using System.Threading.Tasks;
using eLearn.Modules.Users.Core.Commands;
using eLearn.Modules.Users.Core.Dto.Identity.Roles;
using Shared.Infrastructure.Wrapper;

namespace eLearn.Modules.Users.Core.Services
{
    internal interface IRoleService
    {
        Task<Result<List<RoleResponse>>> GetAllAsync();

        Task<int> GetCountAsync();

        Task<Result<RoleResponse>> GetByIdAsync(string id);

        Task<Result<string>> SaveAsync(RoleRequest request);

        Task<Result<string>> DeleteAsync(string id);
    }
}