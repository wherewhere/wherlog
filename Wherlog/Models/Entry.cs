using System.Text.Json.Serialization;

namespace Wherlog.Models
{
    public class Entry<T> : IApi
    {
        [JsonPropertyName("data")]
        public T Data { get; set; }

        [JsonPropertyName("api")]
        public string Api { get; set; }
    }
}
