using System.Collections.Generic;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Shared.Infrastructure;
using Shared.Infrastructure.Modules;

var builder = WebApplication.CreateBuilder(args);
builder.AddModulesConfiguraion();

IList<Assembly> assemblies = ModuleLoader.LoadAssemblies(builder.Configuration, "eLearn.Modules.");
IList<IModule> modules =ModuleLoader.LoadModules(assemblies);
ConfigureService();
var app = builder.Build();
Configure();
app.Run();

void ConfigureService()
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

