using Markdig;
using Markdig.Renderers;
using Markdig.Renderers.Html;

namespace Wherlog.Controls
{
    internal class MarkdownSectionPreCodeExtension : IMarkdownExtension
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

            CodeBlockRenderer originalCodeBlockRenderer = htmlRenderer.ObjectRenderers.FindExact<CodeBlockRenderer>();
            if (originalCodeBlockRenderer != null)
            {
                htmlRenderer.ObjectRenderers.Remove(originalCodeBlockRenderer);
            }

            htmlRenderer.ObjectRenderers.AddIfNotAlready(new MarkdownSectionPreCodeRenderer(
                    new MarkdownSectionPreCodeRendererOptions
                    {
                        PreTagAttributes = "{.snippet .hljs-copy-wrapper}",
                    })
                );
        }
    }
}
