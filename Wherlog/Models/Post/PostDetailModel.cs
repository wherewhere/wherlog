using System;
using System.Text.Json.Serialization;
using Wherlog.Models.Cate;

namespace Wherlog.Models.Post
{
    public sealed class PostDetailModel : IPost, IDetail
    {
        [JsonPropertyName("title")]
        public string Title { get; init; }

        [JsonPropertyName("description")]
        public string Description { get; init; }

        [JsonPropertyName("slug")]
        public string Slug { get; init; }

        [JsonPropertyName("date")]
        public DateTimeOffset Date { get; init; }

        [JsonPropertyName("updated")]
        public DateTimeOffset Updated { get; init; }

        [JsonPropertyName("comments")]
        public bool Comments { get; init; }

        [JsonPropertyName("url")]
        public string Url { get; init; }

        [JsonPropertyName("language")]
        public string Language { get; init; }

        [JsonPropertyName("cover")]
        public string Cover { get; init; }

        [JsonPropertyName("images")]
        public string[] Images { get; init; }

        [JsonPropertyName("content")]
        public string Content { get; init; }

        [JsonPropertyName("raw")]
        public string Raw { get; init; }

        [JsonPropertyName("categories")]
        public CateModel[] Categories { get; init; }

        [JsonPropertyName("tags")]
        public CateModel[] Tags { get; init; }
    }
}
