using System.Text.Json.Serialization;

namespace Wherlog.Models
{
    public class CateModel : IApi
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("api")]
        public string Api { get; set; }
    }
}
