export function queryDomForTocEntries() {
    const article = document.getElementById("article");
    const headings = article.querySelectorAll("h2, h3, h4, h5, h6");
    if (!headings.length) { return []; }

    const levels = [false, false, false, false, false];
    for (const element of headings) {
        if (!element.id) {
            const anchorText = element.innerText;
            const elementId = anchorText.replaceAll(' ', '-', '/', '\\', '#', '$', '@', ':', ',').toLowerCase();
            element.id = elementId;
        }
        if (!levels[0] && element.nodeName === "H2") {
            levels[0] = true;
        }
        else if (!levels[1] && element.nodeName === "H3") {
            levels[1] = true;
        }
        else if (!levels[2] && element.nodeName === "H4") {
            levels[2] = true;
        }
        else if (!levels[3] && element.nodeName === "H5") {
            levels[3] = true;
        }
        else if (!levels[4] && element.nodeName === "H6") {
            levels[4] = true;
        }
    }

    let tag, chapterTag, subchapterTag;
    for (let i = 0; i < levels.length; i++) {
        if (levels[i]) {
            if (!tag) {
                tag = `H${i + 2}`;
            }
            else if (!chapterTag) {
                chapterTag = `H${i + 2}`;
            }
            else if (!subchapterTag) {
                subchapterTag = `H${i + 2}`;
            }
            else {
                break;
            }
        }
    }

    function createAnchor(element) {
        return {
            level: element.nodeName,
            text: element.innerText,
            href: '#' + element.id,
            anchors: []
        };
    }

    const tocArray = [];
    let chapter = null, subchapter = null;
    for (const element of headings) {
        if (element.innerText) {
            if (element.nodeName === tag) {
                chapter = createAnchor(element);
                tocArray.push(chapter);
            }
            else if (chapter && chapterTag && element.nodeName === chapterTag) {
                subchapter = createAnchor(element);
                chapter.anchors.push(subchapter);
            }
            else if (subchapter && subchapterTag && element.nodeName === subchapterTag) {
                subchapter.anchors.push(createAnchor(element));
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
