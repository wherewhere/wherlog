export function highlight(element) {
    if (!(element instanceof Element)) { return; }
    const figure = element.querySelectorAll("figure.highlight");
    figure.forEach(element => {
        // Skip pre > .mermaid for folding and copy button
        if (element.querySelector(".mermaid")) return;
        let span = element.querySelectorAll(".code .line span");
        if (span.length === 0) {
            // Hljs without line_number and wrap
            span = element.querySelectorAll("code.highlight span");
        }
        span.forEach(s => {
            s.classList.forEach(name => {
                if (!name.startsWith("hljs-")) {
                    s.classList.replace(name, `hljs-${name}`);
                }
            });
        });
    });
}

export function fixImage(element) {
    if (element instanceof Element) {
        const images = element.querySelectorAll("img[data-src]:not([src])");
        images.forEach(image => {
            const src = image.getAttribute("data-src");
            if (src) {
                image.removeAttribute("data-src");
                image.setAttribute("src", src);
            }
        });
    }
}