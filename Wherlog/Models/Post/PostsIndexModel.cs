using System.Text.Json.Serialization;

namespace Wherlog.Models.Post
{
    public class PostsIndexModel
    {
        [JsonPropertyName("index")]
        public int Index { get; set; }

        [JsonPropertyName("total")]
        public int Total { get; set; }

        [JsonPropertyName("posts")]
        public PostModel[] Posts { get; set; }

        [JsonPropertyName("info")]
        public InfoModel Info { get; set; }
    }
}
