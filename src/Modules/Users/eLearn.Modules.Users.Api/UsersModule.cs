using System.Collections.Generic;
using eLearn.Modules.Users.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Infrastructure.Modules;

namespace eLearn.Modules.Users.Api
{
    internal class UsersModule : IModule
    {
        public string Name { get; } = "Users";
        
        public IEnumerable<string> Policies { get; } = new[]
        {
            "users"
        };

        public void Register(IServiceCollection services)
        {
            services.AddCore(Name);
        }
        
        public void Use(IConfiguration app)
        {
        }
    }
}