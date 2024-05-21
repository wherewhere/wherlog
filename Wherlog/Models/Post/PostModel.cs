using System;
using System.Text.Json.Serialization;

namespace Wherlog.Models.Post
{
    public class PostModel : IApi
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("date")]
        public DateTimeOffset Date { get; set; }

        [JsonPropertyName("updated")]
        public DateTimeOffset Updated { get; set; }

        [JsonPropertyName("comments")]
        public bool Comments { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }

        [JsonPropertyName("excerpt")]
        public string Excerpt { get; set; }

        [JsonPropertyName("images")]
        public string[] Images { get; set; }

        [JsonPropertyName("categories")]
        public CateModel[] Categories { get; set; }

        [JsonPropertyName("tags")]
        public CateModel[] Tags { get; set; }

        [JsonPropertyName("api")]
        public string Api { get; set; }

        [JsonPropertyName("cover")]
        public string Cover { get; set; }
    }
}
