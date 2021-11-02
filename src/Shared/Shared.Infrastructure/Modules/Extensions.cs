using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace Shared.Infrastructure.Modules
{
    public static class Extensions
    {
        public static WebApplicationBuilder AddModulesConfiguraion(this WebApplicationBuilder builder)
        {
            foreach (var settings in GetSettings("*"))
            {
                builder.Configuration.AddJsonFile(settings);
            }

            IEnumerable<string> GetSettings(string pattern)
                => Directory.EnumerateFiles(builder.Environment.ContentRootPath,
                    $"module.{pattern}.json", SearchOption.AllDirectories);

            return builder;
        }
    }
}