using System;
using System.Text.Json.Serialization;
using Wherlog.Models.Cate;

namespace Wherlog.Models.Post
{
    public interface IPost
    {
        [JsonPropertyName("title")]
        string Title { get; }

        [JsonPropertyName("date")]
        DateTimeOffset Date { get; }

        [JsonPropertyName("updated")]
        DateTimeOffset Updated { get; }

        [JsonPropertyName("comments")]
        bool Comments { get; }

        [JsonPropertyName("url")]
        string Url { get; }

        [JsonPropertyName("language")]
        string Language { get; }

        [JsonPropertyName("cover")]
        string Cover { get; }

        [JsonPropertyName("images")]
        string[] Images { get; }

        [JsonPropertyName("categories")]
        CateModel[] Categories { get; }

        [JsonPropertyName("tags")]
        CateModel[] Tags { get; }
    }

    public interface IPost<out TInfo> : IInfo<TInfo> where TInfo : IInfo
    {
        [JsonPropertyName("posts")]
        PostModel[] Posts { get; }
    }
}
