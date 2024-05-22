using System;
using System.Text.Json.Serialization;

namespace Wherlog.Models.Post
{
    public class PostDetailModel
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("slug")]
        public string Slug { get; set; }

        [JsonPropertyName("date")]
        public DateTimeOffset Date { get; set; }

        [JsonPropertyName("updated")]
        public DateTimeOffset Updated { get; set; }

        [JsonPropertyName("comments")]
        public bool Comments { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }

        [JsonPropertyName("cover")]
        public string Cover { get; set; }

        [JsonPropertyName("images")]
        public string[] Images { get; set; }

        [JsonPropertyName("content")]
        public string Content { get; set; }

        [JsonPropertyName("raw")]
        public string Raw { get; set; }

        [JsonPropertyName("categories")]
        public CateModel[] Categories { get; set; }

        [JsonPropertyName("tags")]
        public CateModel[] Tags { get; set; }

        [JsonPropertyName("api")]
        public string Api { get; set; }
    }
}
