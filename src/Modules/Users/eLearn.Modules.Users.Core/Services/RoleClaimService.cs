using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using eLearn.Modules.Users.Core.Commands;
using eLearn.Modules.Users.Core.Dto.Identity.Roles;
using eLearn.Modules.Users.Core.Entities;
using eLearn.Modules.Users.Core.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Shared.Infrastructure.Auth;
using Shared.Infrastructure.Wrapper;

namespace eLearn.Modules.Users.Core.Services
{
    internal class RoleClaimService : IRoleClaimService
    {
        private readonly RoleManager<AppRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IUsersDbContext _db;
        private readonly IStringLocalizer<RoleClaimService> _localizer;
        private readonly ICurrentUser _currentUserService;

        public RoleClaimService(IMapper mapper,
            IUsersDbContext db,
            IStringLocalizer<RoleClaimService> localizer,
            RoleManager<AppRole> roleManager, 
            UserManager<AppUser> userManager, 
            ICurrentUser currentUserService)
        {
            _mapper = mapper;
            _db = db;
            _localizer = localizer;
            _roleManager = roleManager;
            _userManager = userManager;
            _currentUserService = currentUserService;
        }

        public async Task<Result<List<RoleClaimResponse>>> GetAllAsync()
        {
            var roleClaims = await _db.RoleClaims.AsNoTracking().ToListAsync();
            var roleClaimsResponse = _mapper.Map<List<RoleClaimResponse>>(roleClaims);
            return await Result<List<RoleClaimResponse>>.SuccessAsync(roleClaimsResponse);
        }

        public async Task<int> GetCountAsync()
        {
            return await _db.RoleClaims.AsNoTracking().CountAsync();
        }

        public async Task<Result<RoleClaimResponse>> GetByIdAsync(int id)
        {
            var roleClaim = await _db.RoleClaims.AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == id);
            var roleClaimResponse = _mapper.Map<RoleClaimResponse>(roleClaim);
            return await Result<RoleClaimResponse>.SuccessAsync(roleClaimResponse);
        }

        public async Task<Result<List<RoleClaimResponse>>> GetAllByRoleIdAsync(string roleId)
        {
            var roleClaims = await _db.RoleClaims
                .AsNoTracking()
                .Include(x => x.Role)
                .Where(x => x.RoleId.ToString() == roleId)
                .ToListAsync();
            var roleClaimsResponse = _mapper.Map<List<RoleClaimResponse>>(roleClaims);
            return await Result<List<RoleClaimResponse>>.SuccessAsync(roleClaimsResponse);
        }

        public async Task<Result<string>> SaveAsync(RoleClaimRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.RoleId))
            {
                return await Result<string>.FailAsync(_localizer["Role is required."]);
            }

            if (request.Id == 0)
            {
                var existingRoleClaim =
                    await _db.RoleClaims
                        .SingleOrDefaultAsync(x =>
                            x.RoleId.ToString() == request.RoleId && x.ClaimType == request.Type &&
                            x.ClaimValue == request.Value);
                if (existingRoleClaim != null)
                {
                    return await Result<string>.FailAsync(_localizer["Similar Role Claim already exists."]);
                }

                var roleClaim = _mapper.Map<AppRoleClaim>(request);
                await _db.RoleClaims.AddAsync(roleClaim);
                // roleClaim.AddDomainEvent(new RoleClaimAddedEvent(roleClaim));
                // await _db.SaveChangesAsync();
                return await Result<string>.SuccessAsync(string.Format(_localizer["Role Claim {0} created."],
                    request.Value));
            }
            else
            {
                var existingRoleClaim =
                    await _db.RoleClaims
                        .Include(x => x.Role)
                        .SingleOrDefaultAsync(x => x.Id == request.Id);
                if (existingRoleClaim == null)
                {
                    return await Result<string>.FailAsync(_localizer["Role Claim does not exist."]);
                }
                else
                {
                    existingRoleClaim.ClaimType = request.Type;
                    existingRoleClaim.ClaimValue = request.Value;
                    existingRoleClaim.Group = request.Group;
                    existingRoleClaim.Description = request.Description;
                    existingRoleClaim.RoleId = Convert.ToInt32(request.RoleId);
                    _db.RoleClaims.Update(existingRoleClaim);
                    // existingRoleClaim.AddDomainEvent(new RoleClaimUpdatedEvent(existingRoleClaim));
                    // await _db.SaveChangesAsync();
                    return await Result<string>.SuccessAsync(string.Format(
                        _localizer["Role Claim {0} for Role {1} updated."], request.Value,
                        existingRoleClaim.Role?.Name));
                }
            }
        }

        public async Task<Result<string>> DeleteAsync(int id)
        {
            var existingRoleClaim = await _db.RoleClaims
                .Include(x => x.Role)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (existingRoleClaim != null)
            {
                if (existingRoleClaim.Role?.Name == RoleConstants.SuperAdmin)
                {
                    return await Result<string>.FailAsync(string.Format(
                        _localizer["Not allowed to delete Permissions for {0} Role."], existingRoleClaim.Role.Name));
                }

                _db.RoleClaims.Remove(existingRoleClaim);
                // existingRoleClaim.AddDomainEvent(new RoleClaimDeletedEvent(id));
                // await _db.SaveChangesAsync();
                return await Result<string>.SuccessAsync(string.Format(
                    _localizer["Role Claim {0} for {1} Role deleted."], existingRoleClaim.ClaimValue,
                    existingRoleClaim.Role?.Name));
            }
            else
            {
                return await Result<string>.FailAsync(_localizer["Role Claim does not exist."]);
            }
        }

        public async Task<Result<PermissionResponse>> GetAllPermissionsAsync(string roleId)
        {
            var response = new PermissionResponse
            {
                RoleClaims = new()
            };
            response.RoleClaims.GetAllPermissions();
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role != null)
            {
                response.RoleId = role.Id.ToString();
                response.RoleName = role.Name;
                var allRoleClaims = await GetAllAsync();
                var roleClaimsResult = await GetAllByRoleIdAsync(role.Id.ToString());
                if (roleClaimsResult.Succeeded)
                {
                    var roleClaims = roleClaimsResult.Data;
                    var allClaimValues = response.RoleClaims.Select(a => a.Value).ToList();
                    if (roleClaims != null)
                    {
                        var roleClaimValues = roleClaims.Select(a => a.Value).ToList();
                        var authorizedClaims = allClaimValues.Intersect(roleClaimValues).ToList();
                        foreach (var permission in response.RoleClaims)
                        {
                            permission.Id = allRoleClaims.Data?.SingleOrDefault(x =>
                                x.RoleId == roleId && x.Type == permission.Type && x.Value == permission.Value)?.Id ?? 0;
                            permission.RoleId = roleId;
                            if (authorizedClaims.Any(a => a == permission.Value))
                            {
                                permission.Selected = true;
                                var roleClaim = roleClaims.SingleOrDefault(a =>
                                    a.Type == permission.Type && a.Value == permission.Value);
                                if (roleClaim?.Description != null)
                                {
                                    permission.Description = roleClaim.Description;
                                }

                                if (roleClaim?.Group != null)
                                {
                                    permission.Group = roleClaim.Group;
                                }
                            }
                        }
                    }
                }
                else
                {
                    response.RoleClaims = new();
                    return await Result<PermissionResponse>.FailAsync(roleClaimsResult.Messages);
                }
            }
            else
            {
                response.RoleClaims = new();
                return await Result<PermissionResponse>.FailAsync(_localizer["Role does not exist."]);
            }

            return await Result<PermissionResponse>.SuccessAsync(response);
        }

        public async Task<Result<string>> UpdatePermissionsAsync(PermissionRequest request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.RoleId))
                {
                    return await Result<string>.FailAsync(_localizer["Role is required."]);
                }

                if (request.RoleClaims != null && request.RoleClaims.Any(c => !c.Type.Equals(ApplicationClaimTypes.Permission)))
                {
                    return await Result<string>.FailAsync(string.Format(_localizer["All Role Claims Type values should be '{0}'."], ApplicationClaimTypes.Permission));
                }

                if (request.RoleClaims != null && request.RoleClaims.Any(c => !c.RoleId.Equals(request.RoleId)))
                {
                    return await Result<string>.FailAsync(string.Format(_localizer["All Role Claims should contain the same Role Id as in the request."], ApplicationClaimTypes.Permission));
                }

                var errors = new List<string>();
                var role = await _roleManager.FindByIdAsync(request.RoleId);
                if (role != null)
                {
                    if (role.Name == RoleConstants.SuperAdmin)
                    {
                        var currentUser = await _userManager.Users.SingleAsync(x => x.Id.ToString() == _currentUserService.GetUserId()!.ToString());
                        if (!await _userManager.IsInRoleAsync(currentUser, RoleConstants.SuperAdmin))
                        {
                            return await Result<string>.FailAsync(_localizer["Not allowed to modify Permissions for this Role."]);
                        }
                    }

                    if (request.RoleClaims != null)
                    {
                        var deSelectedClaims = request.RoleClaims.Where(a => !a.Selected).ToList();
                        if (role.Name == RoleConstants.SuperAdmin)
                        {
                            if (deSelectedClaims.Any(x => x.Value == Shared.Infrastructure.Auth.Permissions.Roles.View) ||
                                deSelectedClaims.Any(x => x.Value == Shared.Infrastructure.Auth.Permissions.RoleClaims.View) ||
                                deSelectedClaims.Any(x => x.Value == Shared.Infrastructure.Auth.Permissions.RoleClaims.Edit))
                            {
                                return await Result<string>.FailAsync(string.Format(
                                    _localizer["Not allowed to deselect {0} or {1} or {2} for this Role."],
                                    Shared.Infrastructure.Auth.Permissions.Roles.View,
                                    Shared.Infrastructure.Auth.Permissions.RoleClaims.View,
                                    Shared.Infrastructure.Auth.Permissions.RoleClaims.Edit));
                            }
                        }

                        // delete deselected claims
                        foreach (var claim in deSelectedClaims)
                        {
                            if (claim.Id != 0)
                            {
                                var removeResult = await DeleteAsync(claim.Id);
                                if (!removeResult.Succeeded)
                                {
                                    errors.AddRange(removeResult.Messages);
                                }
                            }
                        }
                    }

                    // add or update selected claims
                    if (request.RoleClaims != null)
                        foreach (var claim in request.RoleClaims.Where(a => a.Selected).ToList())
                        {
                            var saveResult = await SaveAsync(_mapper.Map<RoleClaimRequest>(claim));
                            if (!saveResult.Succeeded)
                            {
                                errors.AddRange(saveResult.Messages);
                            }
                        }

                    if (errors.Count > 0)
                    {
                        return await Result<string>.FailAsync(errors);
                    }

                    return await Result<string>.SuccessAsync(_localizer["Permissions Updated."]);
                }
                else
                {
                    return await Result<string>.FailAsync(_localizer["Role does not exist."]);
                }
            }
            catch (Exception ex)
            {
                return await Result<string>.FailAsync(ex.Message);
            }
        }
    }
}