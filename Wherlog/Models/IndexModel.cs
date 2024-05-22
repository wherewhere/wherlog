using System.Text.Json.Serialization;

namespace Wherlog.Models
{
    public sealed class IndexModel<TInfo> : IApi where TInfo : IInfo
    {
        [JsonPropertyName("index")]
        public int Index { get; init; }

        [JsonPropertyName("count")]
        public int Count { get; init; }

        [JsonPropertyName("api")]
        public string Api { get; init; }

        [JsonPropertyName("info")]
        public TInfo Info { get; init; }
    }

    public class InfoModel : IInfo
    {
        [JsonPropertyName("type")]
        public string Type { get; init; }
    }
}
