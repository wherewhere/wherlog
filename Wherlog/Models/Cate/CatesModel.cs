using System.Text.Json.Serialization;

namespace Wherlog.Models.Cate
{
    public sealed class CatesModel : CateModel
    {
        [JsonPropertyName("slug")]
        public string Slug { get; init; }

        [JsonPropertyName("count")]
        public int Count { get; init; }
    }
}
