using System;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Shared.Infrastructure.Serialization.Settings;

namespace Shared.Infrastructure.Serialization
{
    public class NewtonSoftJsonSerializer : IJsonSerializer
    {
        private readonly JsonSerializerSettings? _settings;

        public NewtonSoftJsonSerializer(IOptions<JsonSerializerSettingsOptions> settings)
        {
            _settings = settings.Value.JsonSerializerSettings;
        }

        public T? Deserialize<T>(string text, IJsonSerializerSettingsOptions settings)
            => JsonConvert.DeserializeObject<T>(text, settings?.JsonSerializerSettings ?? _settings);

        public string Serialize<T>(T obj, IJsonSerializerSettingsOptions settings)
            => JsonConvert.SerializeObject(obj, settings?.JsonSerializerSettings ?? _settings);

        public string Serialize<T>(T obj, Type type, IJsonSerializerSettingsOptions settings)
            => JsonConvert.SerializeObject(obj, type, settings?.JsonSerializerSettings ?? _settings);
    }
}