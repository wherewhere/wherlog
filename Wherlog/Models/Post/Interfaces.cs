using System;
using System.Text.Json.Serialization;

namespace Wherlog.Models.Post
{
    public interface IPost
    {
        [JsonPropertyName("title")]
        public string Title { get; }

        [JsonPropertyName("date")]
        public DateTimeOffset Date { get; }

        [JsonPropertyName("updated")]
        public DateTimeOffset Updated { get; }

        [JsonPropertyName("comments")]
        public bool Comments { get; }

        [JsonPropertyName("url")]
        public string Url { get; }

        [JsonPropertyName("cover")]
        public string Cover { get; }

        [JsonPropertyName("images")]
        public string[] Images { get; }

        [JsonPropertyName("categories")]
        public CateModel[] Categories { get; }

        [JsonPropertyName("tags")]
        public CateModel[] Tags { get; }

        [JsonPropertyName("api")]
        public string Api { get; }
    }
}
