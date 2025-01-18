export function highlight() {
    const preTagList = document.getElementsByTagName("pre");
    const numberOfPreTags = preTagList.length;
    for (let i = 0; i < numberOfPreTags; i++) {
        const codeTag = preTagList[i].getElementsByTagName("code");
        hljs.highlightElement(codeTag[0]);
    }
}

export function addCopyButton() {
    const snippets = document.querySelectorAll(".snippet");
    const numberOfSnippets = snippets.length;
    for (let i = 0; i < numberOfSnippets; i++) {
        if (!snippets[i].querySelector("button.hljs-copy")) {
            const code = snippets[i].querySelector("code").innerText;
            const copyButton = document.createElement("button");
            copyButton.className = "hljs-copy";
            copyButton.innerText = "Copy";
            copyButton.addEventListener("click", (event) => {
                const button = event.target;
                navigator.clipboard.writeText(code)
                    .then(() => {
                        if (button instanceof HTMLElement) {
                            button.innerText = "已复制";
                            setTimeout(() => button.innerText = "Copy", 1000)
                        }
                    });
            });
            snippets[i].appendChild(copyButton);
        }
    }
}
