using Markdig;
using Markdig.Renderers;
using Markdig.Renderers.Html.Inlines;

namespace Wherlog.Controls
{
    public sealed class LinkInlineWithAttributesRendererExtension : IMarkdownExtension
    {
        public void Setup(MarkdownPipelineBuilder pipeline)
        {
        }

        public void Setup(MarkdownPipeline pipeline, IMarkdownRenderer renderer)
        {
            if (renderer is not TextRendererBase<HtmlRenderer> htmlRenderer)
            {
                return;
            }

            LinkInlineRenderer originalLinkInlineRenderer = htmlRenderer.ObjectRenderers.FindExact<LinkInlineRenderer>();
            if (originalLinkInlineRenderer != null)
            {
                _ = htmlRenderer.ObjectRenderers.Remove(originalLinkInlineRenderer);
            }

            htmlRenderer.ObjectRenderers.AddIfNotAlready(
                new LinkInlineWithAttributesRenderer
                {
                    ImageAttributes =
                    {
                        { "loading", "lazy"  }
                    }
                });
        }
    }
}
