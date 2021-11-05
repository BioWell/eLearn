using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using eLearn.Modules.Users.Core.Commands;
using eLearn.Modules.Users.Core.Dto.Users;
using eLearn.Modules.Users.Core.Entities;
using eLearn.Modules.Users.Core.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Shared.Infrastructure.Auth;
using Shared.Infrastructure.Wrapper;

namespace eLearn.Modules.Users.Core.Services
{
    internal class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<UserService> _localizer;

        public UserService(
            UserManager<AppUser> userManager,
            IMapper mapper,
            RoleManager<AppRole> roleManager,
            IStringLocalizer<UserService> localizer)
        {
            _userManager = userManager;
            _mapper = mapper;
            _roleManager = roleManager;
            _localizer = localizer;
        }

        public async Task<Result<List<UserResponse>>> GetAllAsync()
        {
            var users = await _userManager.Users.AsNoTracking().ToListAsync();
            var result = _mapper.Map<List<UserResponse>>(users);
            return await Result<List<UserResponse>>.SuccessAsync(result);
        }

        public async Task<IResult<UserResponse>> GetAsync(string userId)
        {
            var user = await _userManager.Users.AsNoTracking().Where(u => u.Id.ToString() == userId).FirstOrDefaultAsync();
            var result = _mapper.Map<UserResponse>(user);
            return await Result<UserResponse>.SuccessAsync(result);
        }

        public async Task<IResult<UserRolesResponse>> GetRolesAsync(string userId)
        {
            var viewModel = new List<UserRoleModel>();
            var user = await _userManager.FindByIdAsync(userId);
            var roles = await _roleManager.Roles.AsNoTracking().ToListAsync();
            foreach (var role in roles)
            {
                var userRolesViewModel = new UserRoleModel
                {
                    RoleId = role.Id.ToString(),
                    RoleName = role.Name
                };
                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    userRolesViewModel.Selected = true;
                }
                else
                {
                    userRolesViewModel.Selected = false;
                }

                viewModel.Add(userRolesViewModel);
            }

            var result = new UserRolesResponse { UserRoles = viewModel };
            return await Result<UserRolesResponse>.SuccessAsync(result);
        }

        public async Task<IResult<string>> UpdateAsync(UpdateUserRequest request)
        {
            var user = await _userManager.FindByIdAsync(request.Id);

            if (user == null) throw new IdentityException(string.Format(_localizer["User Id {0} is not found."], request.Id));

            _mapper.Map<UpdateUserRequest, AppUser>(request, user);

            // user.AddDomainEvent(new UserUpdatedEvent(user));

            if (request.Password != null)
            {
                await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.Password);
            }

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return await Result<string>.SuccessAsync(user.Id.ToString(),  _localizer["User Updated Succesffully."]);
            }
            else
            {
                throw new IdentityException(_localizer["Validation Errors Occurred."], result.Errors.Select(a => _localizer[a.Description].ToString()).ToList());
            }
        }

        public async Task<IResult<string>> UpdateUserRolesAsync(string userId, UserRolesRequest request)
        {
            var user = await _userManager.Users.Where(u => u.Id.ToString() == userId).FirstOrDefaultAsync();
            if (user == null)
            {
                return await Result<string>.FailAsync(_localizer["User Not Found."]);
            }

            if (await _userManager.IsInRoleAsync(user, RoleConstants.SuperAdmin))
            {
                return await Result<string>.FailAsync(_localizer["Not Allowed."]);
            }

            foreach (var userRole in request.UserRoles)
            {
                // Check if Role Exists
                if (await _roleManager.FindByNameAsync(userRole.RoleName) != null)
                {
                    if (userRole.Selected)
                    {
                        if (!await _userManager.IsInRoleAsync(user, userRole.RoleName))
                        {
                            await _userManager.AddToRoleAsync(user, userRole.RoleName);
                        }
                    }
                    else
                    {
                        await _userManager.RemoveFromRoleAsync(user, userRole.RoleName);
                    }
                }
            }

            return await Result<string>.SuccessAsync(userId, string.Format(_localizer["User Roles Updated Successfully."]));
        }
    }
}