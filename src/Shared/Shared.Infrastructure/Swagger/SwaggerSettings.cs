using System;

namespace Shared.Infrastructure.Swagger
{
    public class SwaggerSettings
    {
        public bool Enabled { get; set; } = true;
        public string Title { get; set; } = String.Empty;
        public string Version { get; set; } = String.Empty;
        public bool IncludeSecurity { get; set; } = true;
        public bool SerializeAsOpenApiV2 { get; set; } = true;
    }
}