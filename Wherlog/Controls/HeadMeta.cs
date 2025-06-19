using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Sections;
using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace Wherlog.Controls
{
    public sealed class HeadMeta : ComponentBase
    {
        private const string JAVASCRIPT_FILE = $"./js/meta-helper.js";

        internal static readonly object DescriptionSectionId = new();
        internal static readonly object ThemeColorsSectionId = new();

        private string _defaultDescription;
        private ThemeColor[] _defaultThemeColors;
        private bool _isInitialized;

        [Inject]
        private IJSRuntime JSRuntime { get; set; }

        /// <inheritdoc/>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await using IJSObjectReference _jsModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", JAVASCRIPT_FILE);
                _defaultDescription = await _jsModule.InvokeAsync<string>("getAndRemoveExistingMeta", "description");
                _defaultThemeColors = await _jsModule.InvokeAsync<ThemeColor[]>("getAndRemoveExistingThemeColor");
                _isInitialized = true;
                StateHasChanged();
            }
        }

        /// <inheritdoc/>
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            if (!_isInitialized) { return; }

            // Render the description content
            builder.OpenComponent<SectionOutlet>(0);
            builder.AddComponentParameter(1, nameof(SectionOutlet.SectionId), DescriptionSectionId);
            builder.CloseComponent();

            // Render the default description if it exists
            if (!string.IsNullOrEmpty(_defaultDescription))
            {
                builder.OpenComponent<SectionContent>(2);
                builder.AddComponentParameter(3, nameof(SectionContent.SectionId), DescriptionSectionId);
                builder.AddComponentParameter(4, "IsDefaultContent", true);
                builder.AddComponentParameter(5, nameof(SectionContent.ChildContent), (RenderFragment)BuildDefaultDescriptionRenderTree);
                builder.CloseComponent();
            }

            // Render the theme-color content
            builder.OpenComponent<SectionOutlet>(6);
            builder.AddComponentParameter(7, nameof(SectionOutlet.SectionId), ThemeColorsSectionId);
            builder.CloseComponent();


            // Render the default theme-color if it exists
            if (_defaultThemeColors?.Length > 0)
            {
                builder.OpenComponent<SectionContent>(8);
                builder.AddComponentParameter(9, nameof(SectionContent.SectionId), ThemeColorsSectionId);
                builder.AddComponentParameter(10, "IsDefaultContent", true);
                builder.AddComponentParameter(11, nameof(SectionContent.ChildContent), (RenderFragment)BuildDefaultThemeColorsRenderTree);
                builder.CloseComponent();
            }
        }

        private void BuildDefaultDescriptionRenderTree(RenderTreeBuilder builder)
        {
            builder.OpenElement(0, "meta");
            builder.AddAttribute(1, "name", "description");
            builder.AddAttribute(2, "content", _defaultDescription);
            builder.CloseElement();
        }

        private void BuildDefaultThemeColorsRenderTree(RenderTreeBuilder builder)
        {
            int index = 0;
            foreach(ThemeColor themeColor in _defaultThemeColors)
            {
                builder.OpenElement(index++, "meta");
                builder.AddAttribute(index++, "name", "theme-color");
                builder.AddAttribute(index++, "content", themeColor.Content);
                if (!string.IsNullOrEmpty(themeColor.Media))
                {
                    builder.AddAttribute(index++, "media", themeColor.Media);
                }
                builder.CloseElement();
            }
        }

        private record ThemeColor(string Content, string Media);
    }
}
