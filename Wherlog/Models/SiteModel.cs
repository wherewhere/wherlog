using System.Collections.Generic;
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

        [JsonPropertyName("favicon")]
        public Dictionary<string, string> Favicon { get; init; }

        [JsonPropertyName("social")]
        public Dictionary<string, IconURL> Social { get; init; }

        [JsonPropertyName("reward")]
        public Dictionary<string, string> Reward { get; init; }

        [JsonPropertyName("follow_me")]
        public Dictionary<string, IconURL> FollowMe { get; init; }
    }
}
