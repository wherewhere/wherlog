using System.Text.Json.Serialization;

namespace Wherlog.Models.Archive
{
    public sealed class MonthInfo : InfoModel
    {
        [JsonPropertyName("year")]
        public int Year { get; init; }

        [JsonPropertyName("month")]
        public int Month { get; init; }
    }
}
