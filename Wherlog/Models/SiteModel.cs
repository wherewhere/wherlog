using System.Text.Json.Serialization;

namespace Wherlog.Models
{
    public class SiteModel
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("subtitle")]
        public string Subtitle { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("author")]
        public string Author { get; set; }

        [JsonPropertyName("language")]
        public string Language { get; set; }

        [JsonPropertyName("timezone")]
        public string TimeZone { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }

        [JsonPropertyName("keywords")]
        public string[] Keywords { get; set; }
    }
}
