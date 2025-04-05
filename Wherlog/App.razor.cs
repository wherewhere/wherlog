using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Globalization;
using System.Threading.Tasks;
using Wherlog.Helpers;

namespace Wherlog
{
    public partial class App
    {
        public static SettingsHelper SettingsHelper { get; private set; }

        public static async Task Main(string[] args)
        {
            WebAssemblyHostBuilder builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            _ = builder.Services
                       .AddLocalization()
                       .AddFluentUIComponents();

            WebAssemblyHost host = builder.Build();
            IJSRuntime jsRuntime = host.Services.GetRequiredService<IJSRuntime>();

            if (SettingsHelper == null)
            {
                SettingsHelper = new SettingsHelper(jsRuntime, host.Services.GetRequiredService<ILogger<SettingsHelper>>());
                await SettingsHelper.SetDefaultSettingsAsync();
            }

            RequestHelper.Default = new RequestHelper(await SettingsHelper.GetAsync<string>(SettingsHelper.BaseUrl));

            string language = await SettingsHelper.GetAsync<string>(SettingsHelper.CurrentLanguage);
            if (language != LanguageHelper.AutoLanguageCode)
            {
                CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.DefaultThreadCurrentCulture = new CultureInfo(language);
            }

            await LanguageHelper.SetLanguageCodeAsync(LanguageHelper.GetCurrentLanguage().ToLowerInvariant(), jsRuntime);

            await host.RunAsync();
        }
    }
}
