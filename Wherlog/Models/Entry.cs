using System.Text.Json.Serialization;

namespace Wherlog.Models
{
    public sealed class Entry<T> : IApi
    {
        [JsonPropertyName("data")]
        public T Data { get; init; }

        [JsonPropertyName("api")]
        public string Api { get; init; }
    }
}
