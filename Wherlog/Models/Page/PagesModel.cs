using System.Text.Json.Serialization;

namespace Wherlog.Models.Page
{
    public sealed class PagesModel : IApi
    {
        [JsonPropertyName("title")]
        public string Title { get; init; }

        [JsonPropertyName("url")]
        public string Url { get; init; }

        [JsonPropertyName("api")]
        public string Api { get; init; }
    }
}
