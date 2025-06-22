// When the user scrolls down 20px from the top of the document, show the button
const bodycontent = document.getElementById("body-content") || document.body;

// When the user clicks on the button, scroll to the top of the document
export function backToTop() {
    bodycontent.scrollTo({ top: 0, behavior: "smooth" });
}

export function registerBackToTopButton(backToTopButton) {
    bodycontent.addEventListener("scroll", () => {
        if (backToTopButton instanceof HTMLElement) {
            const contentHeight = bodycontent.scrollHeight - bodycontent.offsetHeight;
            const scrollPercent = contentHeight > 0 ? Math.min(100 * bodycontent.scrollTop / contentHeight, 100) : 0;
            const isShow = Math.round(scrollPercent) >= 5;
            backToTopButton.classList.toggle("on", isShow);
            backToTopButton.querySelector("span").innerText = Math.round(scrollPercent) + '%';
        }
    });
}
