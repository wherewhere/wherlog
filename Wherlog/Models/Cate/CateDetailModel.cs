using System.Text.Json.Serialization;
using Wherlog.Models.Post;

namespace Wherlog.Models.Cate
{
    public sealed class CateDetailModel : ICount<CateInfoModel>
    {
        [JsonPropertyName("count")]
        public int Count { get; init; }

        [JsonPropertyName("posts")]
        public PostModel[] Posts { get; init; }

        [JsonPropertyName("info")]
        public CateInfoModel Info { get; init; }
    }
}
