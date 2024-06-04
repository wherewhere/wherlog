export function highlight() {
    const preTagList = document.getElementsByTagName('pre');
    const numberOfPreTags = preTagList.length;
    for (let i = 0; i < numberOfPreTags; i++) {
        const codeTag = preTagList[i].getElementsByTagName('code');
        hljs.highlightElement(codeTag[0]);
    }
}

export function addCopyButton() {
    const snippets = document.querySelectorAll('.snippet');
    const numberOfSnippets = snippets.length;
    for (let i = 0; i < numberOfSnippets; i++) {
        let copyButton = snippets[i].getElementsByClassName("hljs-copy")
        if (copyButton.length === 0) {
            let code = snippets[i].getElementsByTagName('code')[0].innerText;
            snippets[i].innerHTML = snippets[i].innerHTML + '<button class="hljs-copy">Copy</button>'; // append copy button

            copyButton[0].addEventListener("click", () => {
                navigator.clipboard.writeText(code);

                this.innerText = 'Copied!';
                let button = this;
                setTimeout(() => button.innerText = 'Copy', 1000)
            });
        }
    }
}
