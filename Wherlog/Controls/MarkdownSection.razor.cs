using Markdig;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;
using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;
using Wherlog.Helpers;

namespace Wherlog.Controls
{
    public sealed partial class MarkdownSection : FluentComponentBase, IAsyncDisposable
    {
        private const string JAVASCRIPT_FILE = $"./{nameof(Controls)}/{nameof(MarkdownSection)}.razor.js";
        private bool _raiseContentConverted;
        private IJSObjectReference _jsModule;

        private string ClassValue => new CssBuilder(Class)
            .AddClass("markdown-section")
            .Build();

        /// <summary>
        /// Gets or sets the Markdown content 
        /// </summary>
        [Parameter]
        public string Content { get; set; }

        [Parameter]
        public bool IsChangeCardTheme { get; set; } = true;

        [Parameter]
        public EventCallback OnContentConverted { get; set; }

        public string InternalContent
        {
            get;
            set
            {
                if (field != value)
                {
                    field = value;

                    HtmlContent = string.IsNullOrWhiteSpace(value)
                        ? new MarkupString(value)
                        : ConvertToMarkupString(field);

                    if (OnContentConverted.HasDelegate)
                    {
                        _ = OnContentConverted.InvokeAsync();
                    }

                    _raiseContentConverted = true;
                    StateHasChanged();
                }
            }
        }

        public MarkupString HtmlContent { get; private set; }

        protected override void OnParametersSet() => InternalContent = Content;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                // add highlight for any code blocks
                _jsModule ??= await JSRuntime.TryInvokeAsync<IJSObjectReference>("import", JAVASCRIPT_FILE);
            }

            if (_raiseContentConverted)
            {
                await _jsModule.TryInvokeVoidAsync("highlight");
                await _jsModule.TryInvokeVoidAsync("addCopyButton");

                if (IsChangeCardTheme)
                {
                    await _jsModule.TryInvokeVoidAsync("setTheme");
                }

                _raiseContentConverted = false;
                if (OnContentConverted.HasDelegate)
                {
                    await OnContentConverted.InvokeAsync();
                }
            }
        }

        private static MarkupString ConvertToMarkupString(string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                MarkdownPipelineBuilder builder = new MarkdownPipelineBuilder()
                        .UseAdvancedExtensions()
                        .Use<LinkInlineWithAttributesRendererExtension>()
                        .Use<MarkdownSectionPreCodeExtension>();

                MarkdownPipeline pipeline = builder.Build();

                // Convert markdown string to HTML
                string html = Markdown.ToHtml(value.ReplaceCDN(), pipeline);

                // Return sanitized HTML as a MarkupString that Blazor can render
                return new MarkupString(html);
            }

            return new MarkupString();
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
                Logger.LogInformation(ex, "JSRuntime has already disconnected. {message} (0x{hResult:X})", ex.GetMessage(), ex.HResult);
            }
        }
    }
}
