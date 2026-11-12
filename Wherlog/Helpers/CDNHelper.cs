using System.Collections.Generic;

namespace Wherlog.Helpers
{
    public static class CDNHelper
    {
        private static readonly Dictionary<string, string> replacement = new()
        {
            { "https://cdn.jsdelivr.net/npm/@fluentui/web-components@v2/+esm", "./_content/Microsoft.FluentUI.AspNetCore.Components/Microsoft.FluentUI.AspNetCore.Components.lib.module.js" },
            { "https://cdn.jsdelivr.net/npm/hexo-tag-bilibili-card/components/bilibili-card/bilibili-card.js", "./lib/hexo-tag-bilibili-card/components/bilibili-card/bilibili-card.js" },
            { "https://cdn.jsdelivr.net/npm/hexo-tag-bilibili-card/components/bilibili-card-message/bilibili-card-message.esm.js", "./lib/hexo-tag-bilibili-card/components/bilibili-card-message/bilibili-card-message.esm.min.js" },
            { "https://cdn.jsdelivr.net/npm/hexo-tag-bilibili-card/components/bilibili-card-builder/bilibili-card-builder.esm.js", "./lib/hexo-tag-bilibili-card/components/bilibili-card-builder/bilibili-card-builder.esm.min.js" },
            { "https://cdn.jsdelivr.net/npm/marked", "./lib/marked/lib/marked.umd.min.js" },
            { "https://cdn.jsdelivr.net/gh/xiapeng01/xiapeng01.GitHub.io@4975fc1/marked.js", "./lib/marked/lib/marked.umd.min.js" },
            { "https://cdn.jsdelivr.net/gh/haldai/org-slides@master/style/fonts/DinkieBitmap-9px.woff2", "./lib/org-slides/style/fonts/DinkieBitmap-9px.woff2" },
            { "https://cdn.jsdelivr.net/npm/swagger-ui-dist/swagger-ui-bundle.js", "./lib/swagger-ui-dist/swagger-ui-bundle.min.js" },
            { "https://cdn.jsdelivr.net/npm/swagger-ui-dist/swagger-ui.css", "./lib/swagger-ui-dist/swagger-ui.css" }
        };

        public static string ReplaceCDN(this string source)
        {
            foreach (KeyValuePair<string, string> kvp in replacement)
            {
                source = source.Replace(kvp.Key, kvp.Value);
            }
            return source;
        }
    }
}
