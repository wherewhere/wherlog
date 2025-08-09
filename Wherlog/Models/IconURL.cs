using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Wherlog.Models
{
    [JsonConverter(typeof(IconURLConverter))]
    public sealed class IconURL
    {
        public IconURL(string value)
        {
            string[] values = value.Split("||", StringSplitOptions.RemoveEmptyEntries);
            switch (values)
            {
                case [string url]:
                    URL = url.Trim();
                    break;
                case [string url, string icon]:
                    URL = url.Trim();
                    Icon = icon.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    break;
                default:
                    throw new ArgumentException("Invalid format for IconURL. Expected 'url || icon' or 'url'.");
            }
        }

        public string URL { get; }
        public string[] Icon { get; }

        public override string ToString() => $"{URL} || {string.Join(' ', Icon)}";

        public sealed class IconURLConverter : JsonConverter<IconURL>
        {
            public override IconURL Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
                reader.GetString() is string value ? new IconURL(value) : null;

            public override void Write(Utf8JsonWriter writer, IconURL value, JsonSerializerOptions options)
            {
                if (value == null)
                {
                    writer.WriteNullValue();
                }
                else
                {
                    writer.WriteStringValue(value.ToString());
                }
            }
        }
    }
}
