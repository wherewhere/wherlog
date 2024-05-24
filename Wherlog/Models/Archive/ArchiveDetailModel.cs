using System.Text.Json.Serialization;
using Wherlog.Models.Post;

namespace Wherlog.Models.Archive
{
    public sealed class ArchiveDetailModel<TInfo> : IArchiveDetail<TInfo> where TInfo : InfoModel
    {
        [JsonPropertyName("count")]
        public int Count { get; init; }

        [JsonPropertyName("posts")]
        public PostModel[] Posts { get; init; }

        [JsonPropertyName("info")]
        public TInfo Info { get; init; }
    }
}
