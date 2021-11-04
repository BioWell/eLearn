using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Shared.Infrastructure.Auth;

namespace eLearn.Modules.Users.Core.Permissions
{
    internal class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
    {
        public PermissionAuthorizationHandler()
        {
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            if (context.User == null)
            {
                await Task.CompletedTask;
            }

            var permissions = context.User?.Claims.Where(x => x.Type == ApplicationClaimTypes.Permission &&
                                                              x.Value == requirement.Permission &&
                                                              x.Issuer == "LOCAL AUTHORITY"); // TO DO
            if (permissions != null && permissions.Any())
            {
                context.Succeed(requirement);
                await Task.CompletedTask;
            }
        }
    }
}