using System;
using System.Text.Json.Serialization;

namespace Wherlog.Models
{
    public interface IApi
    {
        [JsonPropertyName("api")]
        string Api { get; }
    }

    public interface IInfo<out TInfo> where TInfo : IInfo
    {
        [JsonPropertyName("info")]
        TInfo Info { get; }
    }

    public interface IIndex<out TInfo> : IInfo<TInfo> where TInfo : IInfo
    {
        [JsonPropertyName("index")]
        public int Index { get; }
    }

    public interface ICount<out TInfo> : IInfo<TInfo> where TInfo : IInfo
    {
        [JsonPropertyName("count")]
        public int Count { get; }
    }

    public interface IRaw
    {
        [JsonPropertyName("raw")]
        string Raw { get; }

        string GetContent()
        {
            string raw = Raw.Replace("{% raw %}", string.Empty).Replace("{% endraw %}", string.Empty);
            bool inRow = false;
            short time = 0, count = 0;
            int index = 0;
            for (; index < raw.Length; index++)
            {
                char c = raw[index];
                if (inRow)
                {
                    if (c == '-')
                    {
                        count++;
                    }
                    else
                    {
                        if (count >= 3 && ++time == 2)
                        {
                            return raw[index..];
                        }
                        else
                        {
                            inRow = false;
                            count = 0;
                        }
                    }
                }
                else if (c == '-')
                {
                    inRow = true;
                    count++;
                }
            }
            return raw;
        }
    }

    public interface IDetail : IRaw
    {
        [JsonPropertyName("title")]
        public string Title { get; }

        [JsonPropertyName("description")]
        public string Description { get; }

        [JsonPropertyName("date")]
        public DateTimeOffset Date { get; }

        [JsonPropertyName("updated")]
        public DateTimeOffset Updated { get; }

        [JsonPropertyName("language")]
        public string Language { get; }

        [JsonPropertyName("comments")]
        public bool Comments { get; }

        [JsonPropertyName("url")]
        public string Url { get; }

        [JsonPropertyName("cover")]
        public string Cover { get; }

        [JsonPropertyName("images")]
        public string[] Images { get; }

        [JsonPropertyName("content")]
        public string Content { get; }
    }

    public interface IInfo
    {
        [JsonPropertyName("type")]
        string Type { get; }
    }
}
