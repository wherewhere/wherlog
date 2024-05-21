using System.Text.Json.Serialization;

namespace Wherlog.Models
{
    public class IndexModel<TInfo> : IApi where TInfo : IInfo
    {
        [JsonPropertyName("index")]
        public int Index { get; set; }

        [JsonPropertyName("count")]
        public int Count { get; set; }

        [JsonPropertyName("api")]
        public string Api { get; set; }

        [JsonPropertyName("info")]
        public TInfo Info { get; set; }
    }

    public class InfoModel : IInfo
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }
    }
}
