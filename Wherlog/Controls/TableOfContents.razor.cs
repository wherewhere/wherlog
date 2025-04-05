using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.Extensions.Logging;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wherlog.Helpers;

namespace Wherlog.Controls
{
    public partial class TableOfContents : IAsyncDisposable
    {
        private const string JAVASCRIPT_FILE = $"./{nameof(Controls)}/{nameof(TableOfContents)}.razor.js";

        private record Anchor(string Level, string Text, string Href, Anchor[] Anchors)
        {
            public virtual bool Equals(Anchor other)
            {
                if (other is null)
                {
                    return false;
                }

                if (Level != other.Level ||
                    Text != other.Text ||
                    Href != other.Href ||
                    (Anchors?.Length ?? 0) != (other.Anchors?.Length ?? 0))
                {
                    return false;
                }

                if (Anchors is not null &&
                    Anchors.Length > 0)
                {
                    for (int i = 0; i < Anchors.Length; i++)
                    {
                        if (!Anchors[i].Equals(other.Anchors[i]))
                        {
                            return false;
                        }
                    }
                }

                return true;
            }

            public override int GetHashCode()
                 => HashCode.Combine(Level, Text, Href);
        }

        private Anchor[] _anchors;
        private bool _expanded = false;
        private bool _isMobile = false;

        private IJSObjectReference _jsModule;

        /// <summary>
        /// Gets or sets the heading for the ToC 
        /// Defaults to 'In this article'
        /// </summary>
        [Parameter]
        public string Heading { get; set; } = "In this article";

        /// <summary>
        /// Gets or sets a value indicating whether a 'Back to top' button should be rendered.
        /// Defaults to true
        /// </summary>
        [Parameter]
        public bool ShowBackButton { get; set; } = true;

        /// <summary>
        /// Gets or sets the content to be rendered inside the component.
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                _jsModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", JAVASCRIPT_FILE);
                _isMobile = await _jsModule.InvokeAsync<bool>("isDevice");

                await BackToTopAsync();
                await QueryDomAsync();
            }
        }

        private async Task BackToTopAsync()
        {
            if (_jsModule is null)
            {
                return;
            }
            _ = await _jsModule.InvokeAsync<Anchor[]>("backToTop");
        }

        private async Task QueryDomAsync()
        {
            if (_jsModule is null)
            {
                return;
            }

            Anchor[] foundAnchors = await _jsModule.InvokeAsync<Anchor[]>("queryDomForTocEntries");

            if (AnchorsEqual(_anchors, foundAnchors))
            {
                return;
            }

            _anchors = foundAnchors;
            _expanded = !_isMobile && _anchors?.Length > 0;
            StateHasChanged();
        }

        private static bool AnchorsEqual(Anchor[] firstSet, Anchor[] secondSet) =>
            (firstSet ?? [])
                .SequenceEqual(secondSet ?? []);

        protected override void OnInitialized()
        {
            // Subscribe to the event
            NavigationManager.LocationChanged += LocationChanged;
        }

        private async void LocationChanged(object sender, LocationChangedEventArgs e)
        {
            try
            {
                await BackToTopAsync();
                await QueryDomAsync();
            }
            catch (ObjectDisposedException)
            {
                // Already disposed
            }
        }

        public Task Refresh() => QueryDomAsync();

        private RenderFragment GetTocItems(IEnumerable<Anchor> items)
        {
            return items is not null
                ? new RenderFragment(builder =>
                {
                    int i = 0;
                    builder.OpenElement(i++, "ul");
                    foreach (Anchor item in items)
                    {
                        builder.OpenElement(i++, "li");
                        builder.OpenComponent<FluentAnchor>(i++);
                        builder.AddAttribute(i++, "Href", item.Href);
                        builder.AddAttribute(i++, "Appearance", Appearance.Hypertext);
                        builder.AddAttribute(i++, "ChildContent", (RenderFragment)(content =>
                        {
                            content.AddContent(i++, item.Text);
                        }));
                        builder.CloseComponent();
                        if (item.Anchors is not null)
                        {
                            builder.AddContent(i++, GetTocItems(item.Anchors));
                        }
                        builder.CloseElement();
                    }
                    builder.CloseElement();
                })
                : new RenderFragment(builder =>
                {
                    builder.AddContent(0, ChildContent);
                });
        }

        public async ValueTask DisposeAsync()
        {
            try
            {
                // Unsubscribe from the event when our component is disposed
                NavigationManager.LocationChanged -= LocationChanged;

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
