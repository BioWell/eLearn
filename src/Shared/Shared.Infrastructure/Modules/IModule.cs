using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Shared.Infrastructure.Modules
{
    public interface IModule
    {
        string Name { get; }
        IEnumerable<string>? Policies => null;
        void Register(IServiceCollection services);
        void Use(IConfiguration app);        
    }
}