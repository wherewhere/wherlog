import builder from "../../lib/hexo-tag-bilibili-card/components/bilibili-card-builder/bilibili-card-builder.esm.min.js";

export function createCard(imageProxy, infoTypes, message, theme) {
    const card = builder.createCardWithTagName("div", imageProxy, infoTypes, message, theme);
    if (card instanceof HTMLElement) {
        const link = document.createElement("link");
        link.rel = "stylesheet";
        link.href = getTheme(theme || '0');
        card.insertBefore(link, card.firstChild);
        return card.innerHTML;
    }
}

function getTheme(theme) {
    const baseUrl = new URL("lib/hexo-tag-bilibili-card/components/bilibili-card/bilibili-card", document.baseURI);
    switch (theme.toLowerCase()) {
        case '1':
        case "light":
            return `${baseUrl}.light.css`;
        case '2':
        case "dark":
            return `${baseUrl}.dark.css`;
        case '0':
        case "auto":
        case "system":
        case "default":
            return `${baseUrl}.css`;
        case "fd":
        case "fd2":
        case "fluent":
        case "fluentui":
            return `${baseUrl}.fluent.css`;
        case "windose":
            return `${baseUrl}.windose.css`;
        case "-1":
        case "none":
            return '';
        default:
            return theme;
    }
}