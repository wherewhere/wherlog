using Markdig;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;
using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace Wherlog.Controls
{
    public partial class MarkdownSection : FluentComponentBase, IAsyncDisposable
    {
        private const string JAVASCRIPT_FILE = $"./{nameof(Controls)}/{nameof(MarkdownSection)}.razor.js";

        private string _content;
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
        public EventCallback OnContentConverted { get; set; }

        public string InternalContent
        {
            get => _content;
            set
            {
                if (_content != value)
                {
                    _content = value;

                    HtmlContent = string.IsNullOrWhiteSpace(value)
                        ? new MarkupString(value)
                        : ConvertToMarkupString(_content);

                    if (OnContentConverted.HasDelegate)
                    {
                        OnContentConverted.InvokeAsync();
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
                _jsModule ??= await JSRuntime.InvokeAsync<IJSObjectReference>("import", JAVASCRIPT_FILE);
            }

            if (_raiseContentConverted)
            {
                await _jsModule.InvokeVoidAsync("highlight");
                await _jsModule.InvokeVoidAsync("addCopyButton");

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
                        .Use<MarkdownSectionPreCodeExtension>();

                MarkdownPipeline pipeline = builder.Build();

                // Convert markdown string to HTML
                string html = Markdown.ToHtml(value, pipeline);

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
            }
        }
    }
}
