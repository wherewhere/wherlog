using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Wherlog.Controls
{
    public partial class SettingsCard : FluentComponentBase
    {
        private string ClassValue => new CssBuilder(Class)
            .AddClass("settings-card")
            .Build();

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
        /// Gets or sets the icon on the left.
        /// </summary>
        [Parameter]
        public RenderFragment Icon { get; set; }

        /// <summary>
        /// Gets or sets the icon that is shown when IsClickEnabled is set to true.
        /// </summary>
        [Parameter]
        public RenderFragment ActionIcon { get; set; }

        /// <summary>
        /// Gets or sets the content of a ContentControl.
        /// </summary>
        [Parameter]
        public RenderFragment ActionContent { get; set; }

        /// <summary>
        /// Gets or sets the tooltip of the ActionIcon.
        /// </summary>
        [Parameter]
        public string ActionIconToolTip { get; set; } = "More";

        /// <summary>
        /// Gets or sets if the card can be clicked.
        /// </summary>
        [Parameter]
        public bool IsClickEnabled { get; set; }

        /// <summary>
        /// Gets or sets the alignment of the Content.
        /// </summary>
        [Parameter]
        public ContentAlignment ContentAlignment { get; set; }
    }

    public enum ContentAlignment
    {
        /// <summary>
        /// The Content is aligned to the right. Default state.
        /// </summary>
        Right,
        /// <summary>
        /// The Content is left-aligned while the Header, HeaderIcon and Description are collapsed. This is commonly used for Content types such as CheckBoxes, RadioButtons and custom layouts.
        /// </summary>
        Left,
        /// <summary>
        /// The Content is vertically aligned.
        /// </summary>
        Vertical
    }
}
