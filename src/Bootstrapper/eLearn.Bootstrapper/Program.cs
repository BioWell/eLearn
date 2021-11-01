using System.Collections.Generic;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Shared.Infrastructure;
using Shared.Infrastructure.Modules;

var builder = WebApplication.CreateBuilder(args);
builder.AddModulesConfiguraion();

IList<Assembly> assemblies = ModuleLoader.LoadAssemblies(builder.Configuration, "eLearn.Modules.");
IList<IModule> modules =ModuleLoader.LoadModules(assemblies);
ConfigureService(builder.Services);
var app = builder.Build();
Configure();
assemblies.Clear();
modules.Clear();
app.Run();

void ConfigureService(IServiceCollection services)
{
    builder.Services.AddModularInfrastructure();
    
    // modules
    //     .AddIdentityModule(_config)
    //     .AddSharedApplication(_config)
    //     .AddCatalogModule(_config)
    //     .AddPeopleModule(_config)
    //     .AddSalesModule(_config)
    //     .AddInventoryModule(_config);
    
    foreach (var module in modules)
    {
        module.Register(builder.Services);
    }
}

void Configure()
{
    app.UseModularInfrastructure();
    foreach (var module in modules)
    {
        module.Use(builder.Configuration);
    }
}

