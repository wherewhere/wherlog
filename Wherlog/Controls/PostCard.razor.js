export function highlight() {
    const elements = document.getElementsByClassName('highlight');
    const numberOfElements = elements.length;
    for (let i = 0; i < numberOfElements; i++) {
        setElements(elements[i]);
    }
    function setElements(element) {
        if (element instanceof Element) {
            const className = [];
            element.classList.forEach(x => className.push('hljs-' + x));
            element.className = className.join(' ');
            const numberOfElements = +element?.children.length;
            for (let i = 0; i < numberOfElements; i++) {
                setElements(element.children[i]);
            }
        }
    }
}

export function fixImage() {
    const images = document.getElementsByTagName('img');
    const numberOfElements = images.length;
    for (let i = 0; i < numberOfElements; i++) {
        const image = images[i];
        const src = image.getAttribute('data-src');
        if (src) {
            image.removeAttribute('data-src');
            image.setAttribute('src', src);
        }
    }
}