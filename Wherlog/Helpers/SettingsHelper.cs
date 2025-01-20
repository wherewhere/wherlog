using Microsoft.JSInterop;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using System.Threading.Tasks;

namespace Wherlog.Helpers
{
    public sealed class SettingsHelper(IJSRuntime jsRuntime) : IAsyncDisposable
    {
        private const string JAVASCRIPT_FILE = $"./js/settings-helper.js";

        private IJSObjectReference _jsModule;

        public const string BaseUrl = nameof(BaseUrl);
        public const string CurrentLanguage = nameof(CurrentLanguage);

        public async ValueTask<Type> GetAsync<Type>(string key)
        {
            _jsModule ??= await jsRuntime.InvokeAsync<IJSObjectReference>("import", JAVASCRIPT_FILE);
            string value = await _jsModule.InvokeAsync<string>("getValue", key);
            if (string.IsNullOrEmpty(value)) { return default; }
            System.Type type = typeof(Type);
            return type == typeof(string) ? Deserialize(value, SourceGenerationContext.Default.String)
                : JsonSerializer.Deserialize<Type>(value);
            static Type Deserialize<TValue>([StringSyntax(StringSyntaxAttribute.Json)] string json, JsonTypeInfo<TValue> jsonTypeInfo) => JsonSerializer.Deserialize(json, jsonTypeInfo) is Type value ? value : default;
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

        public async ValueTask ResetAsync()
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

            if (!await IsExistsAsync(BaseUrl))
            {
                await SetAsync(BaseUrl, RequestHelper.DefaultBaseUrl);
            }
            if (!await IsExistsAsync(CurrentLanguage))
            {
                await SetAsync(CurrentLanguage, LanguageHelper.AutoLanguageCode);
            }
        }

        public async ValueTask DisposeAsync()
        {
            try
            {
                if (_jsModule != null)
                {
                    await _jsModule.DisposeAsync();
                    _jsModule = null;
                }

                GC.SuppressFinalize(this);
            }
            catch (Exception ex) when (ex is JSDisconnectedException or
                                       OperationCanceledException)
            {
                // The JSRuntime side may routinely be gone already if the reason we're disposing is that
                // the client disconnected. This is not an error.
            }
        }
    }
}
