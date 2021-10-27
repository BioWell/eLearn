using Microsoft.AspNetCore.Builder;
using Shared.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
ConfigureService();
var app = builder.Build();
Configure();
app.Run();

void ConfigureService()
{
    builder.Services.AddModularInfrastructure();
}

void Configure()
{
    app.UseModularInfrastructure();
}