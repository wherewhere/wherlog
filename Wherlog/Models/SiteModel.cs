using System.Text.Json.Serialization;

namespace Wherlog.Models
{
    public sealed class SiteModel
    {
        [JsonPropertyName("title")]
        public string Title { get; init; }

        [JsonPropertyName("subtitle")]
        public string Subtitle { get; init; }

        [JsonPropertyName("description")]
        public string Description { get; init; }

        [JsonPropertyName("author")]
        public string Author { get; init; }

        [JsonPropertyName("language")]
        public string Language { get; init; }

        [JsonPropertyName("timezone")]
        public string TimeZone { get; init; }

        [JsonPropertyName("url")]
        public string Url { get; init; }

        [JsonPropertyName("keywords")]
        public string[] Keywords { get; init; }
    }
}
