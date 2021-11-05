using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using eLearn.Modules.Users.Core.Dto.Identity.Roles;
using eLearn.Modules.Users.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Shared.Infrastructure.Auth;

namespace eLearn.Modules.Users.Core.Services
{
    internal static class Extensions
    {
        public static void GetAllPermissions(this List<RoleClaimModel> allPermissions)
        {
            foreach (var module in typeof(Shared.Infrastructure.Auth.Permissions).GetNestedTypes())
            {
                string moduleName = String.Empty;
                string moduleDescription = String.Empty;

                if (module.GetCustomAttributes(typeof(DisplayNameAttribute), true)
                    .FirstOrDefault() is DisplayNameAttribute displayNameAttribute)
                {
                    moduleName = displayNameAttribute.DisplayName;
                }

                if (module.GetCustomAttributes(typeof(DescriptionAttribute), true)
                    .FirstOrDefault() is DescriptionAttribute descriptionAttribute)
                {
                    moduleDescription = descriptionAttribute.Description;
                }

                foreach (var fi in module.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy))
                {
                    object? propertyValue = fi.GetValue(null);

                    if (propertyValue is not null)
                    {
                        allPermissions.Add(new() { Value = propertyValue.ToString(), Type = ApplicationClaimTypes.Permission, Group = moduleName, Description = moduleDescription });
                    }
                }
            }
        }
        
        public static async Task<IdentityResult> AddPermissionClaimAsync(this RoleManager<AppRole> roleManager, AppRole role, string permission)
        {
            var allClaims = await roleManager.GetClaimsAsync(role);
            if (!allClaims.Any(a => a.Type == ApplicationClaimTypes.Permission && a.Value == permission))
            {
                return await roleManager.AddClaimAsync(role, new(ApplicationClaimTypes.Permission, permission));
            }

            return IdentityResult.Failed();
        }
    }
}