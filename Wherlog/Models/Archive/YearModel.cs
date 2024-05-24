using System.Text.Json.Serialization;

namespace Wherlog.Models.Archive
{
    public sealed class YearModel : IApi
    {
        [JsonPropertyName("year")]
        public int Year { get; init; }

        [JsonPropertyName("api")]
        public string Api { get; init; }

        [JsonPropertyName("data")]
        public MonthModel[] Data { get; init; }
    }
}
