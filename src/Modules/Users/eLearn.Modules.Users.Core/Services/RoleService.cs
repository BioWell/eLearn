using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using eLearn.Modules.Users.Core.Commands;
using eLearn.Modules.Users.Core.Dto.Identity.Roles;
using eLearn.Modules.Users.Core.Entities;
using eLearn.Modules.Users.Core.Exceptions;
using eLearn.Modules.Users.Core.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Shared.Infrastructure.Constants;
using Shared.Infrastructure.Utilities;
using Shared.Infrastructure.Wrapper;

namespace eLearn.Modules.Users.Core.Services
{
    internal class RoleService : IRoleService
    {
        private readonly RoleManager<AppRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<RoleService> _localizer;

        public RoleService(RoleManager<AppRole> roleManager,
            IMapper mapper,
            IStringLocalizer<RoleService> localizer,
            UserManager<AppUser> userManager)
        {
            _roleManager = roleManager;
            _mapper = mapper;
            _localizer = localizer;
            _userManager = userManager;
        }

        public async Task<Result<List<RoleResponse>>> GetAllAsync()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            var rolesResponse = _mapper.Map<List<RoleResponse>>(roles);
            return await Result<List<RoleResponse>>.SuccessAsync(rolesResponse);
        }

        public async Task<int> GetCountAsync()
        {
            return await _roleManager.Roles.CountAsync();
        }

        public async Task<Result<RoleResponse>> GetByIdAsync(string id)
        {
            var roles = await _roleManager.Roles.SingleOrDefaultAsync(x => x.Id.ToString() == id);
            var rolesResponse = _mapper.Map<RoleResponse>(roles);
            return await Result<RoleResponse>.SuccessAsync(rolesResponse);
        }

        public async Task<Result<string>> SaveAsync(RoleRequest request)
        {
            if (string.IsNullOrEmpty(request.Id))
            {
                var existingRole = await _roleManager.FindByNameAsync(request.Name);
                if (existingRole != null)
                {
                    throw new IdentityException(_localizer["Similar Role already exists."],
                        statusCode: System.Net.HttpStatusCode.BadRequest);
                }

                var newRole = new AppRole()
                {
                    Name = request.Name, 
                    Description = request.Description
                };
                
                var response = await _roleManager.CreateAsync(newRole);
                // newRole.AddDomainEventdDomainEvent(new RoleAddedEvent(newRole));
                //await _context.SaveChangesAsync();
                if (response.Succeeded)
                {
                    return await Result<string>.SuccessAsync(newRole.Id.ToString(),
                        string.Format(_localizer["Role {0} Created."], request.Name));
                }
                else
                {
                    return await Result<string>.FailAsync(response.Errors
                        .Select(e => _localizer[e.Description].ToString()).ToList());
                }
            }
            else
            {
                var existingRole = await _roleManager.FindByIdAsync(request.Id);
                if (existingRole == null)
                {
                    return await Result<string>.FailAsync(_localizer["Role does not exist."]);
                }

                if (DefaultRoles().Contains(existingRole.Name))
                {
                    return await Result<string>.SuccessAsync(
                        string.Format(_localizer["Not allowed to modify {0} Role."], existingRole.Name));
                }

                existingRole.Name = request.Name;
                existingRole.NormalizedName = request.Name.ToUpper();
                existingRole.Description = request.Description;
                // existingRole.AddDomainEvent(new RoleUpdatedEvent(existingRole));
                await _roleManager.UpdateAsync(existingRole);
                return await Result<string>.SuccessAsync(existingRole.Id.ToString(),
                    string.Format(_localizer["Role {0} Updated."], existingRole.Name));
            }
        }

        public async Task<Result<string>> DeleteAsync(string id)
        {
            var existingRole = await _roleManager.FindByIdAsync(id);
            if (existingRole == null)
            {
                throw new IdentityException("Role Not Found", statusCode: System.Net.HttpStatusCode.NotFound);
            }

            if (DefaultRoles().Contains(existingRole.Name))
            {
                return await Result<string>.FailAsync(string.Format(_localizer["Not allowed to delete {0} Role."],
                    existingRole.Name));
            }

            bool roleIsNotUsed = true;
            var allUsers = await _userManager.Users.ToListAsync();
            foreach (var user in allUsers)
            {
                if (await _userManager.IsInRoleAsync(user, existingRole.Name))
                {
                    roleIsNotUsed = false;
                }
            }

            if (roleIsNotUsed)
            {
                // existingRole.AddDomainEvent(new RoleDeletedEvent(id));
                await _roleManager.DeleteAsync(existingRole);
                return await Result<string>.SuccessAsync(existingRole.Id.ToString(),
                    string.Format(_localizer["Role {0} Deleted."], existingRole.Name));
            }
            else
            {
                return await Result<string>.FailAsync(
                    string.Format(_localizer["Not allowed to delete {0} Role as it is being used."],
                        existingRole.Name));
            }
        }

        private static List<string> DefaultRoles()
        {
            return typeof(RoleConstants).GetAllPublicConstantValues<string>();
        }
    }
}