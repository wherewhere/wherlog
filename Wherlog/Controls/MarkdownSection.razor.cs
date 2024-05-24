using Markdig;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace Wherlog.Controls
{
    public partial class MarkdownSection : FluentComponentBase
    {
        private string _content;
        private bool _raiseContentConverted;
        private IJSObjectReference _jsModule = default;

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
                _content = value;
                HtmlContent = ConvertToMarkupString(_content);

                if (OnContentConverted.HasDelegate)
                {
                    OnContentConverted.InvokeAsync();
                }
                _raiseContentConverted = true;
                StateHasChanged();
            }
        }

        public MarkupString HtmlContent { get; private set; }

        protected override void OnInitialized()
        {
            if (Content == null)
            {
                throw new ArgumentNullException(nameof(Content),"You need to provide either Content or FromAsset parameter");
            }

            InternalContent = Content;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                // add highlight for any code blocks
                _jsModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./Controls/MarkdownSection.razor.js");
                await _jsModule.InvokeVoidAsync("highlight");
                await _jsModule.InvokeVoidAsync("addCopyButton");
            }

            if (_raiseContentConverted)
            {
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
    }
}
