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
    updateActiveNav();
});

// When the user clicks on the button, scroll to the top of the document
export function backToTop() {
    bodycontent.scrollTo({ top: 0, behavior: "smooth" });
}

// Very simple check to see if mobile or tablet is being used 
export function isDevice() {
    return /android|webos|iphone|ipad|ipod|blackberry|iemobile|opera mini|mobile/i.test(navigator.userAgent);
}

let sections = [];
function updateActiveNav() {
    if (!sections.length) { return; }
    let index = sections.findIndex(x => x?.getBoundingClientRect().top > 50);
    if (index === -1) {
        index = sections.length - 1;
    } else if (index > 0) {
        index--;
    }
    activateNavByIndex(index);
}

function activateNavByIndex(index) {
    const nav = document.querySelector(".table-of-contents ul");
    if (!nav) { return; }

    const navItemList = nav.getElementsByTagName("li");
    const target = navItemList[index];

    if (!target || target.classList.contains("active-current")) { return; }

    const actives = nav.querySelectorAll(".active");
    actives.forEach(x => x.classList.remove("active", "active-current"));
    target.classList.add("active", "active-current");

    let activateEle = target.parentElement;

    while (nav.contains(activateEle)) {
        if (activateEle instanceof HTMLLIElement) {
            activateEle.classList.add("active");
        }
        activateEle = activateEle.parentElement;
    }
}

export function registerSidebarTOC() {
    sections = [];
    const elements = document.querySelectorAll(".table-of-contents li fluent-anchor");
    elements.forEach(element => {
        const target = document.getElementById(decodeURI(element.getAttribute("href")).replace('#', ''));
        sections.push(target);
    });
    updateActiveNav();
}
