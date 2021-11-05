using System;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Shared.Infrastructure.Auth;

namespace eLearn.Modules.Users.Core.Services
{
    internal class CurrentUser : ICurrentUser
    {
        private readonly IHttpContextAccessor _accessor;

        public CurrentUser(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        public string? Name => _accessor.HttpContext?.User.Identity?.Name;

        public string? GetUserId()
        {
            return IsAuthenticated() ? _accessor.HttpContext?.User.GetUserId() : String.Empty;
        }

        public string? GetUserEmail()
        {
            return IsAuthenticated() ? _accessor.HttpContext?.User.GetUserEmail() : String.Empty;
        }

        public bool IsAuthenticated()
        {
            return _accessor.HttpContext?.User.Identity?.IsAuthenticated ?? false;
        }

        public bool IsInRole(string role)
        {
            return _accessor.HttpContext?.User.IsInRole(role) ?? false;
        }

        public IEnumerable<Claim>? GetUserClaims()
        {
            return _accessor.HttpContext?.User.Claims;
        }

        public HttpContext? GetHttpContext()
        {
            return _accessor.HttpContext;
        }
    }
}