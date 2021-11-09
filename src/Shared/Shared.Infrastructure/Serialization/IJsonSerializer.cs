using System;
using Shared.Infrastructure.Serialization.Settings;

namespace Shared.Infrastructure.Serialization
{
    public interface IJsonSerializer
    {
        string Serialize<T>(T obj, IJsonSerializerSettingsOptions? settings);

        string Serialize<T>(T obj, Type type, IJsonSerializerSettingsOptions? settings);

        T? Deserialize<T>(string text, IJsonSerializerSettingsOptions? settings );
    }
}