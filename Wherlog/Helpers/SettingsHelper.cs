using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.JavaScript;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

namespace Wherlog.Helpers
{
    public static partial class SettingsHelper
    {
        public const string BaseUrl = nameof(BaseUrl);
        public const string CurrentLanguage = nameof(CurrentLanguage);
        public const string UseCalendarArchive = nameof(UseCalendarArchive);

        public static Type Get<Type>(string key)
        {
            string value = GetItem(key);
            if (string.IsNullOrEmpty(value)) { return default; }
            System.Type type = typeof(Type);
            return type == typeof(bool) ? Deserialize(value, SourceGenerationContext.Default.Boolean)
                : type == typeof(string) ? Deserialize(value, SourceGenerationContext.Default.String)
                : JsonSerializer.Deserialize(value, type, SourceGenerationContext.Default) is Type result ? result : default;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            static Type Deserialize<TValue>([StringSyntax(StringSyntaxAttribute.Json)] string json, JsonTypeInfo<TValue> jsonTypeInfo) =>
                JsonSerializer.Deserialize(json, jsonTypeInfo) is Type value ? value : default;
        }

        public static void Set<Type>(string key, Type value)
        {
            string result = value switch
            {
                bool => JsonSerializer.Serialize(value, SourceGenerationContext.Default.Boolean),
                string => JsonSerializer.Serialize(value, SourceGenerationContext.Default.String),
                _ => JsonSerializer.Serialize(value, typeof(Type), SourceGenerationContext.Default)
            };
            SetItem(key, result);
        }

        public static void Reset() => Clear();

        public static bool IsExists(string key)
        {
            string value = GetItem(key);
            return !string.IsNullOrEmpty(value);
        }

        public static void SetDefaultSettings()
        {
            if (!IsExists(BaseUrl))
            {
                Set(BaseUrl, RequestHelper.DefaultBaseUrl);
            }
            if (!IsExists(CurrentLanguage))
            {
                Set(CurrentLanguage, LanguageHelper.AutoLanguageCode);
            }
            if (!IsExists(UseCalendarArchive))
            {
                Set(UseCalendarArchive, false);
            }
        }

        [JSImport("globalThis.localStorage.getItem")]
        private static partial string GetItem(string key);
        [JSImport("globalThis.localStorage.setItem")]
        private static partial void SetItem(string key, string value);
        [JSImport("globalThis.localStorage.clear")]
        private static partial void Clear();
    }
}
