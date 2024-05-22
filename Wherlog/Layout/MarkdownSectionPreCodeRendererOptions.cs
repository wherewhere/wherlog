namespace Wherlog.Layout
{
    /// <summary>
    /// Options for MarkdownSectionPreCodeRenderer
    /// </summary>
    internal class MarkdownSectionPreCodeRendererOptions
    {
        /// <summary>
        /// html attributes for Tag element in markdig generic attributes format
        /// </summary>
        public string PreTagAttributes { get; init; }
        /// <summary>
        /// html attributes for Code element in markdig generic attributes format
        /// </summary>
        public string CodeTagAttributes { get; init; }
    }
}
