using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Wherlog.Controls
{
    public partial class SettingsPresenter : FluentComponentBase
    {
        private string ClassValue => new CssBuilder(Class)
            .AddClass("settings-presenter")
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
    }
}
