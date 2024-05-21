using System.Text.Json.Serialization;

namespace Wherlog.Models.Post
{
    public class PostsModel
    {
        [JsonPropertyName("total")]
        public int Total { get; set; }

        [JsonPropertyName("count")]
        public int Count { get; set; }

        [JsonPropertyName("pages")]
        public IndexModel<InfoModel>[] Pages { get; set; }

        [JsonPropertyName("info")]
        public InfoModel Info { get; set; }
    }
}
