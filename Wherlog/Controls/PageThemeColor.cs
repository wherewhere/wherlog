using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Sections;
using Microsoft.FluentUI.AspNetCore.Components;

namespace Wherlog.Controls
{
    public sealed class PageThemeColor : ComponentBase
    {
        [Parameter]
        public string LightColor { get; set; }

        [Parameter]
        public string DarkColor { get; set; }

        [Parameter]
        public DesignThemeModes Mode { get; set; }

        /// <inheritdoc/>
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            builder.OpenComponent<SectionContent>(0);
            builder.AddComponentParameter(1, nameof(SectionContent.SectionId), HeadMeta.ThemeColorsSectionId);
            builder.AddComponentParameter(2, nameof(SectionContent.ChildContent), (RenderFragment)BuildDefaultThemeColorsRenderTree);
            builder.CloseComponent();
        }

        private void BuildDefaultThemeColorsRenderTree(RenderTreeBuilder builder)
        {
            switch (Mode)
            {
                case DesignThemeModes.System:
                    if (!string.IsNullOrEmpty(LightColor))
                    {
                        builder.OpenElement(0, "meta");
                        builder.AddAttribute(1, "name", "theme-color");
                        builder.AddAttribute(2, "content", LightColor);
                        builder.AddAttribute(3, "media", "(prefers-color-scheme: light)");
                        builder.CloseElement();
                    }

                    if (!string.IsNullOrEmpty(DarkColor))
                    {
                        builder.OpenElement(4, "meta");
                        builder.AddAttribute(5, "name", "theme-color");
                        builder.AddAttribute(6, "content", DarkColor);
                        builder.AddAttribute(7, "media", "(prefers-color-scheme: dark)");
                        builder.CloseElement();
                    }
                    break;
                case DesignThemeModes.Light:
                    if (!string.IsNullOrEmpty(LightColor))
                    {
                        builder.OpenElement(0, "meta");
                        builder.AddAttribute(1, "name", "theme-color");
                        builder.AddAttribute(2, "content", LightColor);
                        builder.CloseElement();
                    }
                    break;
                case DesignThemeModes.Dark:
                    if (!string.IsNullOrEmpty(DarkColor))
                    {
                        builder.OpenElement(0, "meta");
                        builder.AddAttribute(1, "name", "theme-color");
                        builder.AddAttribute(2, "content", DarkColor);
                        builder.CloseElement();
                    }
                    break;
            }
        }
    }
}
