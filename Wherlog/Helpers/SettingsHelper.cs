using Microsoft.JSInterop;
using System.Text.Json;
using System.Threading.Tasks;

namespace Wherlog.Helpers
{
    public class SettingsHelper(IJSRuntime jsRuntime)
    {
        private const string JAVASCRIPT_FILE = $"./js/settings-helper.js";

        private IJSObjectReference _jsModule;

        public const string CurrentLanguage = nameof(CurrentLanguage);

        public async ValueTask<Type> GetAsync<Type>(string key)
        {
            _jsModule ??= await jsRuntime.InvokeAsync<IJSObjectReference>("import", JAVASCRIPT_FILE);
            string value = await _jsModule.InvokeAsync<string>("getValue", key);
            if (string.IsNullOrEmpty(value)) { return default; }
            System.Type type = typeof(Type);
            return type == typeof(string) && JsonSerializer.Deserialize(value, SourceGenerationContext.Default.String) is Type @string
                ? @string
                : JsonSerializer.Deserialize<Type>(value);
        }

        public async ValueTask SetAsync<Type>(string key, Type value)
        {
            _jsModule ??= await jsRuntime.InvokeAsync<IJSObjectReference>("import", JAVASCRIPT_FILE);
            string result = value switch
            {
                string => JsonSerializer.Serialize(value, SourceGenerationContext.Default.String),
                _ => JsonSerializer.Serialize(value)
            };
            await _jsModule.InvokeVoidAsync("setValue", key, result);
        }

        public async ValueTask ResetAsync<Type>()
        {
            _jsModule ??= await jsRuntime.InvokeAsync<IJSObjectReference>("import", JAVASCRIPT_FILE);
            await _jsModule.InvokeVoidAsync("clear");
        }

        public async Task<bool> IsExistsAsync(string key)
        {
            _jsModule ??= await jsRuntime.InvokeAsync<IJSObjectReference>("import", JAVASCRIPT_FILE);
            string value = await _jsModule.InvokeAsync<string>("getValue", key);
            return !string.IsNullOrEmpty(value);
        }

        public async ValueTask SetDefaultSettingsAsync()
        {
            _jsModule ??= await jsRuntime.InvokeAsync<IJSObjectReference>("import", JAVASCRIPT_FILE);

            if (!await IsExistsAsync(CurrentLanguage))
            {
                await SetAsync(CurrentLanguage, LanguageHelper.AutoLanguageCode);
            }
        }
    }
}
