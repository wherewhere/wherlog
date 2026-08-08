using Markdig.Renderers;
using Markdig.Renderers.Html.Inlines;
using Markdig.Syntax.Inlines;
using System.Collections.Generic;

namespace Wherlog.Controls
{
    internal class LinkInlineWithAttributesRenderer : LinkInlineRenderer
    {
        public Dictionary<string, string> ImageAttributes { get; } = [];

        protected override void Write(HtmlRenderer renderer, LinkInline link)
        {
            if (renderer.EnableHtmlForInline)
            {
                _ = renderer.Write(link.IsImage ? "<img src=\"" : "<a href=\"")
                            .WriteEscapeUrl(link.GetDynamicUrl != null ? link.GetDynamicUrl() ?? link.Url : link.Url)
                            .Write('"')
                            .WriteAttributes(link);
            }
            if (link.IsImage)
            {
                if (renderer.EnableHtmlForInline)
                {
                    _ = renderer.Write(" alt=\"");
                }
                bool wasEnableHtmlForInline = renderer.EnableHtmlForInline;
                renderer.EnableHtmlForInline = false;
                renderer.WriteChildren(link);
                renderer.EnableHtmlForInline = wasEnableHtmlForInline;
                if (renderer.EnableHtmlForInline)
                {
                    _ = renderer.Write('"');
                }
            }

            if (renderer.EnableHtmlForInline && !string.IsNullOrEmpty(link.Title))
            {
                _ = renderer.Write(" title=\"")
                            .WriteEscape(link.Title)
                            .Write('"');
            }

            if (link.IsImage)
            {
                if (renderer.EnableHtmlForInline)
                {
                    foreach (KeyValuePair<string, string> attribute in ImageAttributes)
                    {
                        _ = renderer.Write(' ')
                                    .Write(attribute.Key)
                                    .Write("=\"")
                                    .WriteEscape(attribute.Value)
                                    .Write('"');
                    }
                    _ = renderer.Write(" />");
                }
            }
            else
            {
                if (renderer.EnableHtmlForInline)
                {
                    if (!string.IsNullOrWhiteSpace(Rel))
                    {
                        _ = renderer.Write(" rel=\"")
                                    .Write(Rel)
                                    .Write('"');
                    }
                    _ = renderer.Write('>');
                }
                renderer.WriteChildren(link);
                if (renderer.EnableHtmlForInline)
                {
                    _ = renderer.Write("</a>");
                }
            }
        }
    }
}
