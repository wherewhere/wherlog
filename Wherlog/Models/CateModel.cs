using System.Text.Json.Serialization;

namespace Wherlog.Models
{
    public sealed class CateModel : IApi
    {
        [JsonPropertyName("name")]
        public string Name { get; init; }

        [JsonPropertyName("api")]
        public string Api { get; init; }
    }
}
