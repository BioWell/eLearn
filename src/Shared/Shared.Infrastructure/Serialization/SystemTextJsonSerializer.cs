using System;
using System.Text.Json;
using Microsoft.Extensions.Options;
using Shared.Infrastructure.Serialization.Settings;

namespace Shared.Infrastructure.Serialization
{
    public class SystemTextJsonSerializer : IJsonSerializer
    {
        private readonly JsonSerializerOptions? _options;

        public SystemTextJsonSerializer(IOptions<JsonSerializerSettingsOptions> options)
        {
            _options = options.Value.JsonSerializerOptions;
        }

        public T? Deserialize<T>(string data, IJsonSerializerSettingsOptions? options)
            => JsonSerializer.Deserialize<T>(data, options?.JsonSerializerOptions ?? _options);

        public string Serialize<T>(T data, IJsonSerializerSettingsOptions? options)
            => JsonSerializer.Serialize(data, options?.JsonSerializerOptions ?? _options);

        public string Serialize<T>(T data, Type type, IJsonSerializerSettingsOptions? options)
            => JsonSerializer.Serialize(data, type, options?.JsonSerializerOptions ?? _options);
    }
}