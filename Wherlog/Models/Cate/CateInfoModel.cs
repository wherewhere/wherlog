using System.Text.Json.Serialization;

namespace Wherlog.Models.Cate
{
    public sealed class CateInfoModel : InfoModel
    {
        [JsonPropertyName("name")]
        public string Name { get; init; }

        [JsonPropertyName("slug")]
        public string Slug { get; init; }
    }
}
