using System.Text.Json.Serialization;

namespace Wherlog.Models.Page
{
    public class PagesModel : IApi
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }

        [JsonPropertyName("api")]
        public string Api { get; set; }
    }
}
