using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Sections;

namespace Wherlog.Controls
{
    public sealed class PageDescription : ComponentBase
    {
        [Parameter]
        public string Content { get; set; }

        /// <inheritdoc/>
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            builder.OpenComponent<SectionContent>(0);
            builder.AddComponentParameter(1, nameof(SectionContent.SectionId), HeadMeta.DescriptionSectionId);
            builder.AddComponentParameter(2, nameof(SectionContent.ChildContent), (RenderFragment)BuildDescriptionRenderTree);
            builder.CloseComponent();
        }

        private void BuildDescriptionRenderTree(RenderTreeBuilder builder)
        {
            builder.OpenElement(0, "meta");
            builder.AddAttribute(1, "name", "description");
            builder.AddAttribute(2, "content", Content);
            builder.CloseElement();
        }
    }
}
