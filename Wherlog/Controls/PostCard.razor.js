export function highlight() {
    const elements = document.getElementsByClassName('highlight');
    const numberOfElements = elements.length;
    for (let i = 0; i < numberOfElements; i++) {
        setElements(elements[i]);
    }
    function setElements(element) {
        if (typeof element === 'undefined' || element === null) {
            return;
        }
        if (element?.className) {
            element.className = 'hljs-' + element.className;
        }
        const numberOfElements = +element?.children.length;
        for (let i = 0; i < numberOfElements; i++) {
            setElements(element.children[i]);
        }
    }
}

export function fixEmoji() {
    const images = document.getElementsByClassName('emoji');
    const numberOfElements = images.length;
    for (let i = 0; i < numberOfElements; i++) {
        const image = images[i];
        if (image.tagName === 'IMG') {
            const src = image.getAttribute('data-src');
            if (src) {
                image.removeAttribute('data-src');
                image.setAttribute('src', src);
            }
        }
    }
}