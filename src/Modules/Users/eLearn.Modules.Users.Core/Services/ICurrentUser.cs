using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace eLearn.Modules.Users.Core.Services
{
    internal interface ICurrentUser
    {
        string? Name { get; }

        string? GetUserId();

        string? GetUserEmail();

        bool IsAuthenticated();

        bool IsInRole(string role);

        IEnumerable<Claim>? GetUserClaims();

        HttpContext? GetHttpContext();
    }
}