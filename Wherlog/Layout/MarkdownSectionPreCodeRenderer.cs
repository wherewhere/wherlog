// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Markdig.Extensions.GenericAttributes;
using Markdig.Helpers;
using Markdig.Parsers;
using Markdig.Renderers;
using Markdig.Renderers.Html;
using Markdig.Syntax;
using System;
using System.Collections.Generic;

namespace Wherlog.Layout
{
    /// <summary>
    /// Modified version of original markdig CodeBlockRenderer
    /// </summary>
    /// <see href="https://github.com/xoofx/markdig/blob/master/src/Markdig/Renderers/Html/CodeBlockRenderer.cs"/>
    internal class MarkdownSectionPreCodeRenderer(MarkdownSectionPreCodeRendererOptions options) : HtmlObjectRenderer<CodeBlock>
    {
        private HashSet<string> _blocksAsDiv;

        public bool OutputAttributesOnPre { get; set; }

        /// <summary>
        /// Gets a map of fenced code block infos that should be rendered as div blocks instead of pre/code blocks.
        /// </summary>
        public HashSet<string> BlocksAsDiv => _blocksAsDiv ??= new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        protected override void Write(HtmlRenderer renderer, CodeBlock obj)
        {
            renderer.EnsureLine();

            if (_blocksAsDiv is not null && obj is FencedCodeBlock { Info: string info } && _blocksAsDiv.Contains(info))
            {
                string infoPrefix = (obj.Parser as FencedCodeBlockParser)?.InfoPrefix ??
                                 FencedCodeBlockParser.DefaultInfoPrefix;

                // We are replacing the HTML attribute `language-mylang` by `mylang` only for a div block
                // NOTE that we are allocating a closure here

                if (renderer.EnableHtmlForBlock)
                {
                    renderer.Write("<div")
                            .WriteAttributes(obj.TryGetAttributes(),
                                cls => cls.StartsWith(infoPrefix, StringComparison.Ordinal) ? cls[infoPrefix.Length..] : cls)
                            .Write('>');
                }

                renderer.WriteLeafRawLines(obj, true, true, true);

                if (renderer.EnableHtmlForBlock)
                {
                    renderer.WriteLine("</div>");
                }
            }
            else
            {
                if (renderer.EnableHtmlForBlock)
                {
                    renderer.Write("<pre");

                    WritePreAttributes(renderer, obj, options?.PreTagAttributes);

                    renderer.Write("><code");

                    WriteCodeAttributes(renderer, obj, options?.CodeTagAttributes);

                    renderer.Write('>');
                }

                renderer.WriteLeafRawLines(obj, true, true);

                if (renderer.EnableHtmlForBlock)
                {
                    renderer.WriteLine("</code></pre>");
                }
            }

            renderer.EnsureLine();
        }

        private void WritePreAttributes(HtmlRenderer renderer, CodeBlock obj, string preGenericAttributes)
        {
            HtmlAttributes orig = null;

            if (OutputAttributesOnPre)
            {
                orig = obj.TryGetAttributes();
            }

            WriteElementAttributes(renderer, orig, preGenericAttributes);
        }

        private void WriteCodeAttributes(HtmlRenderer renderer, CodeBlock obj, string codeGenericAttributes)
        {
            HtmlAttributes orig = null;

            if (!OutputAttributesOnPre)
            {
                orig = obj.TryGetAttributes();
            }

            WriteElementAttributes(renderer, orig, codeGenericAttributes);
        }
        private static void WriteElementAttributes(HtmlRenderer renderer, HtmlAttributes fromCodeBlock, string genericAttributes)
        {
            // origin code block had no attributes
            fromCodeBlock ??= new HtmlAttributes();

            // append if any additional attributes provided
            StringSlice ss = new(genericAttributes);
            if (!ss.IsEmpty && GenericAttributesParser.TryParse(ref ss, out HtmlAttributes attributes))
            {
                if (fromCodeBlock != null)
                {
                    if (attributes.Classes != null)
                    {
                        foreach (string a in attributes.Classes)
                        {
                            fromCodeBlock.AddClass(a);
                        }
                    }
                    if (attributes.Properties != null)
                    {
                        foreach (KeyValuePair<string, string> pr in attributes.Properties)
                        {
                            fromCodeBlock.AddProperty(pr.Key, pr.Value!);
                        }
                    }
                }
            }

            //
            renderer.WriteAttributes(fromCodeBlock);
        }
    }
}
