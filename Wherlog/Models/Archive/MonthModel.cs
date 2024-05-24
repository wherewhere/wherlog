using System.Text.Json.Serialization;

namespace Wherlog.Models.Archive
{
    public sealed class MonthModel : IApi
    {
        [JsonPropertyName("month")]
        public int Month { get; init; }

        [JsonPropertyName("api")]
        public string Api { get; init; }
    }
}
