export function highlight() {
    const preTagList = document.getElementsByTagName("pre");
    const numberOfPreTags = preTagList.length;
    for (let i = 0; i < numberOfPreTags; i++) {
        const codeTag = preTagList[i].getElementsByTagName("code");
        hljs.highlightElement(codeTag[0]);
    }
}

export function addCopyButton() {
    const snippets = document.getElementsByClassName("snippet");
    const numberOfSnippets = snippets.length;
    for (let i = 0; i < numberOfSnippets; i++) {
        const snippet = snippets[i];
        if (!snippet.querySelector("button.hljs-copy")) {
            const code = snippet.querySelector("code").innerText;
            const copyButton = document.createElement("button");
            copyButton.className = "hljs-copy";
            copyButton.innerText = "Copy";
            copyButton.addEventListener("click", event => {
                const button = event.target;
                navigator.clipboard.writeText(code)
                    .then(() => {
                        if (button instanceof HTMLElement) {
                            button.innerText = "已复制";
                            setTimeout(() => button.innerText = "Copy", 1000)
                        }
                    });
            });
            snippet.appendChild(copyButton);
        }
    }
}

export function setTheme() {
    const cards = document.getElementsByTagName("bilibili-card");
    for (let i = 0; i < cards.length; i++) {
        const card = cards[i];
        if (!card.hasAttribute("theme")) {
            card.setAttribute("theme", "fluent");
        }
    }
}