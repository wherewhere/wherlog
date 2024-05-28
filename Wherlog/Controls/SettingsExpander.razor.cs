using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace Wherlog.Controls
{
    public partial class SettingsExpander : FluentComponentBase
    {
        /// <summary>
        /// Gets or sets the Header.
        /// </summary>
        [Parameter]
        public RenderFragment Header { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        [Parameter]
        public RenderFragment Description { get; set; }

        /// <summary>
        /// Gets or sets the content of a ContentControl.
        /// </summary>
        [Parameter]
        public RenderFragment Content { get; set; }

        /// <summary>
        /// Gets or sets the icon on the left.
        /// </summary>
        [Parameter]
        public RenderFragment Icon { get; set; }

        /// <summary>
        /// Gets or sets the content of a ContentControl.
        /// </summary>
        [Parameter]
        public RenderFragment ActionContent { get; set; }

        /// <summary>
        /// Gets or sets the alignment of the Content.
        /// </summary>
        [Parameter]
        public ContentAlignment ContentAlignment { get; set; }
    }
}
