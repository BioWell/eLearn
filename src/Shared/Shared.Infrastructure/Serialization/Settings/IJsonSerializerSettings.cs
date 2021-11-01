using Newtonsoft.Json;

namespace Shared.Infrastructure.Serialization.Settings
{
    public interface IJsonSerializerSettings
    {
        public JsonSerializerSettings JsonSerializerSettings { get; }
    }
}