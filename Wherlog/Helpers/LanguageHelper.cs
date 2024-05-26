using Microsoft.JSInterop;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Wherlog.Helpers
{
    public static class LanguageHelper
    {
        private const string JAVASCRIPT_FILE = $"./js/language-helper.js";

        public const string AutoLanguageCode = "auto";
        public const string FallbackLanguageCode = "zh-CN";

        public static readonly string[] SupportLanguages =
        [
            "en-US",
            "zh-CN"
        ];

        private static readonly string[] SupportLanguageCodes =
        [
            "en, en-au, en-ca, en-gb, en-ie, en-in, en-nz, en-sg, en-us, en-za, en-bz, en-hk, en-id, en-jm, en-kz, en-mt, en-my, en-ph, en-pk, en-tt, en-vn, en-zw, en-053, en-021, en-029, en-011, en-018, en-014",
            "zh-Hans, zh-cn, zh-hans-cn, zh-sg, zh-hans-sg"
        ];

        public static CultureInfo[] SupportCultures { get; } = SupportLanguages.Select(x => new CultureInfo(x)).ToArray();

        public static int FindIndexFromSupportLanguageCodes(string language) => Array.FindIndex(SupportLanguageCodes, code => code.Split(',', ' ').Any(x => x.Equals(language, StringComparison.OrdinalIgnoreCase)));

        public static string GetCurrentLanguage()
        {
            CultureInfo language = CultureInfo.CurrentCulture;
            int temp = FindIndexFromSupportLanguageCodes(language.Name);
            return temp != -1 ? SupportLanguages[temp] : FallbackLanguageCode;
        }

        public static string GetPrimaryLanguage()
        {
            CultureInfo language = CultureInfo.DefaultThreadCurrentCulture;
            if (language == null) { return GetCurrentLanguage(); }
            int temp = FindIndexFromSupportLanguageCodes(language.Name);
            return temp == -1 ? FallbackLanguageCode : SupportLanguages[temp];
        }

        public static async ValueTask<string> GetLanguageCodeAsync(string code, IJSRuntime jsRuntime)
        {
            IJSObjectReference _jsModule = await jsRuntime.InvokeAsync<IJSObjectReference>("import", JAVASCRIPT_FILE);
            return await _jsModule.InvokeAsync<string>("getLanguageCode", code);
        }

        public static async ValueTask SetLanguageCodeAsync(string code, IJSRuntime jsRuntime)
        {
            IJSObjectReference _jsModule = await jsRuntime.InvokeAsync<IJSObjectReference>("import", JAVASCRIPT_FILE);
            await _jsModule.InvokeVoidAsync("setLanguageCode", code);
        }
    }
}
