using System.Text.Json;

namespace Shared.Infrastructure.Serialization.Settings
{
    public interface IJsonSerializerOptions
    {
        public JsonSerializerOptions JsonSerializerOptions { get; }
    }
}