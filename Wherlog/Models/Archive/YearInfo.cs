using System.Text.Json.Serialization;

namespace Wherlog.Models.Archive
{
    public sealed class YearInfo : InfoModel
    {
        [JsonPropertyName("year")]
        public int Year { get; init; }

        [JsonPropertyName("months")]
        public MonthModel[] Months { get; init; }
    }
}
