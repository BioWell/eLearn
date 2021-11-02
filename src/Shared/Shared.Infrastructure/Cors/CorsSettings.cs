﻿using System.Collections.Generic;

namespace Shared.Infrastructure.Cors
{
    public sealed class CorsSettings
    {
        public bool AllowCredentials { get; set; }
        public IEnumerable<string>? AllowedOrigins { get; set; }
        public IEnumerable<string>? AllowedMethods { get; set; }
        public IEnumerable<string>? AllowedHeaders { get; set; }
        public IEnumerable<string>? ExposedHeaders { get; set; }
    }
}