using System.Text.Json.Serialization;

namespace Wherlog.Models.Post
{
    public sealed class PostsIndexModel
    {
        [JsonPropertyName("index")]
        public int Index { get; init; }

        [JsonPropertyName("total")]
        public int Total { get; init; }

        [JsonPropertyName("posts")]
        public PostModel[] Posts { get; init; }

        [JsonPropertyName("info")]
        public InfoModel Info { get; init; }
    }
}
