using System;
using System.Text.Json.Serialization;
using Wherlog.Models.Cate;

namespace Wherlog.Models.Post
{
    public sealed class PostModel : IPost, IApi
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

        [JsonPropertyName("excerpt")]
        public string Excerpt { get; init; }

        [JsonPropertyName("images")]
        public string[] Images { get; init; }

        [JsonPropertyName("categories")]
        public CateModel[] Categories { get; init; }

        [JsonPropertyName("tags")]
        public CateModel[] Tags { get; init; }

        [JsonPropertyName("api")]
        public string Api { get; init; }

        [JsonPropertyName("cover")]
        public string Cover { get; init; }
    }
}
