export function queryDomForTocEntries() {
    const article = document.getElementById("article");
    const headings = article.querySelectorAll("h2, h3, h4");

    const tocArray = [];
    let chapter = null;
    let subchapter = null;

    for (let element of headings) {
        if (!element.id) {
            const anchorText = element.innerText;
            const elementId = anchorText.replaceAll(' ', '-', '/', '\\', '#', '$', '@', ':', ',').toLowerCase();
            element.id = elementId;
        }
        if (element.innerText) {
            const anchor = {
                level: element.nodeName,
                text: element.innerText,
                href: '#' + element.id,
                anchors: []
            };

            if ("H3" === element.nodeName) {
                if (chapter) {
                    subchapter = anchor;
                    chapter.anchors.push(subchapter);
                }
            } else if ("H4" === element.nodeName) {
                if (subchapter) {
                    subchapter.anchors.push(anchor);
                }
            }
            else {
                chapter = anchor;
                tocArray.push(chapter);
            }
        }
    }
    return tocArray;
}

const backToTopButton = document.getElementById("backtotop");

// When the user scrolls down 20px from the top of the document, show the button
const bodycontent = document.getElementById("body-content") || document.body;

bodycontent.addEventListener("scroll", () => {
    const contentHeight = bodycontent.scrollHeight - bodycontent.offsetHeight;
    const scrollPercent = contentHeight > 0 ? Math.min(100 * bodycontent.scrollTop / contentHeight, 100) : 0;
    const isShow = Math.round(scrollPercent) >= 5;
    backToTopButton.classList.toggle("on", isShow);
    backToTopButton.querySelector("span").innerText = Math.round(scrollPercent) + '%';
});

// When the user clicks on the button, scroll to the top of the document
export function backToTop() {
    bodycontent.scrollTo({ top: 0, behavior: "smooth" });
}

// Very simple check to see if mobile or tablet is being used 
export function isDevice() {
    return /android|webos|iphone|ipad|ipod|blackberry|iemobile|opera mini|mobile/i.test(navigator.userAgent);
}
