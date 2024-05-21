using System.Text.Json.Serialization;

namespace Wherlog.Models
{
    public interface IApi
    {
        [JsonPropertyName("api")]
        string Api { get; }
    }

    public interface IInfo
    {
        [JsonPropertyName("type")]
        string Type { get; }
    }
}
