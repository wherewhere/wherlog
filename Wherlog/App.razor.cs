using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FluentUI.AspNetCore.Components;
using System.Globalization;
using System.Threading.Tasks;
using Wherlog.Helpers;

namespace Wherlog
{
    public partial class App
    {
        public static SettingsHelper SettingsHelper { get; private set; }

        public static Task Main(string[] args)
        {
            WebAssemblyHostBuilder builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            builder.Services
                   .AddLocalization()
                   .AddFluentUIComponents();

            return builder.Build().RunAsync();
        }

        protected override async Task OnInitializedAsync()
        {
            if (SettingsHelper == null)
            {
                SettingsHelper = new SettingsHelper(JSRuntime);
                await SettingsHelper.SetDefaultSettingsAsync();
            }

            string language = await SettingsHelper.GetAsync<string>(SettingsHelper.CurrentLanguage);
            if (language != LanguageHelper.AutoLanguageCode)
            {
                CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.DefaultThreadCurrentCulture = new CultureInfo(language);
            }

            await LanguageHelper.SetLanguageCodeAsync(LanguageHelper.GetCurrentLanguage().ToLowerInvariant(), JSRuntime);
        }
    }
}
