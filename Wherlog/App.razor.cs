using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Globalization;
using System.Threading.Tasks;
using Wherlog.Helpers;

namespace Wherlog
{
    public partial class App : IAsyncDisposable
    {
        private const string JAVASCRIPT_FILE = $"./{nameof(App)}.razor.js";

        private IJSObjectReference _jsModule;

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

            SettingsHelper.SetDefaultSettings();

            RequestHelper.Default = new RequestHelper(SettingsHelper.Get<string>(SettingsHelper.BaseUrl));

            string language = SettingsHelper.Get<string>(SettingsHelper.CurrentLanguage);
            if (language != LanguageHelper.AutoLanguageCode)
            {
                CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.DefaultThreadCurrentCulture = new CultureInfo(language);
            }

            await LanguageHelper.SetLanguageCodeAsync(LanguageHelper.GetCurrentLanguage().ToLowerInvariant(), jsRuntime);

            await host.RunAsync();
        }

        protected override async Task OnInitializedAsync()
        {
            // add highlight for any code blocks
            _jsModule ??= await JSRuntime.InvokeAsync<IJSObjectReference>("import", JAVASCRIPT_FILE);
            Navigation.LocationChanged += OnLocationChanged;
        }

        private async void OnLocationChanged(object sender, LocationChangedEventArgs args) => await _jsModule.InvokeVoidAsync("onLocationChanged");

        public async ValueTask DisposeAsync()
        {
            try
            {
                Navigation.LocationChanged -= OnLocationChanged;

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
                Logger.LogWarning(ex, "JSRuntime has already disconnected. {message} (0x{hResult:X})", ex.GetMessage(), ex.HResult);
            }
        }
    }
}
