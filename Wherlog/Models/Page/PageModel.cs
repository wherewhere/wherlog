using System.Text.Json.Serialization;

namespace Wherlog.Models.Page
{
    public sealed class PageModel : IApi
    {
        [JsonPropertyName("title")]
        public string Title { get; init; }

        [JsonPropertyName("description")]
        public string Description { get; init; }

        [JsonPropertyName("language")]
        public string Language { get; init; }

        [JsonPropertyName("url")]
        public string Url { get; init; }

        [JsonPropertyName("api")]
        public string Api { get; init; }
    }
}
