using System.Collections.Generic;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Shared.Infrastructure;
using Shared.Infrastructure.Logging;
using Shared.Infrastructure.Modules;

var builder = WebApplication.CreateBuilder(args);
builder.AddModulesConfiguraion();
IList<Assembly> assemblies = ModuleLoader.LoadAssemblies(builder.Configuration, "eLearn.Modules.");
IList<IModule> modules =ModuleLoader.LoadModules(assemblies);
ConfigureService(builder.Services);
builder.Host.ConfigureLogging();
var app = builder.Build();
Configure();
assemblies.Clear();
modules.Clear();
app.Run();

void ConfigureService(IServiceCollection services)
{
    builder.Services.AddModularInfrastructure();
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

