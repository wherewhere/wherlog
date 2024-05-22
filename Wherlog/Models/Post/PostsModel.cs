using System.Text.Json.Serialization;

namespace Wherlog.Models.Post
{
    public sealed class PostsModel
    {
        [JsonPropertyName("total")]
        public int Total { get; init; }

        [JsonPropertyName("count")]
        public int Count { get; init; }

        [JsonPropertyName("pages")]
        public IndexModel<InfoModel>[] Pages { get; init; }

        [JsonPropertyName("info")]
        public InfoModel Info { get; init; }
    }
}
