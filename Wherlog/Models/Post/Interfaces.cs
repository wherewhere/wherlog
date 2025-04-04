using System;
using System.Text.Json.Serialization;
using Wherlog.Models.Cate;

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

        [JsonPropertyName("language")]
        public string Language { get; }

        [JsonPropertyName("cover")]
        public string Cover { get; }

        [JsonPropertyName("images")]
        public string[] Images { get; }

        [JsonPropertyName("categories")]
        public CateModel[] Categories { get; }

        [JsonPropertyName("tags")]
        public CateModel[] Tags { get; }
    }

    public interface IPost<out TInfo> : IInfo<TInfo> where TInfo : IInfo
    {
        [JsonPropertyName("posts")]
        PostModel[] Posts { get; }
    }
}
