using System;
using System.Text.Json.Serialization;

namespace Wherlog.Models.Page
{
    public sealed class PageDetailModel : IDetail
    {
        [JsonPropertyName("title")]
        public string Title { get; init; }

        [JsonPropertyName("date")]
        public DateTimeOffset Date { get; init; }

        [JsonPropertyName("updated")]
        public DateTimeOffset Updated { get; init; }

        [JsonPropertyName("comments")]
        public bool Comments { get; init; }

        [JsonPropertyName("url")]
        public string Url { get; init; }

        [JsonPropertyName("cover")]
        public string Cover { get; init; }

        [JsonPropertyName("images")]
        public string[] Images { get; init; }

        [JsonPropertyName("content")]
        public string Content { get; init; }

        [JsonPropertyName("raw")]
        public string Raw { get; init; }
    }
}
