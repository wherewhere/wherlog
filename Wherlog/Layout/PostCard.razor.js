export function highlight() {
    var elements = document.getElementsByClassName('highlight');
    var numberOfElements = elements.length;
    for (var i = 0; i < numberOfElements; i++) {
        setElements(elements[i]);
    }
    function setElements(element) {
        if (typeof element === 'undefined' || element === null) {
            return;
        }
        if (element?.className) {
            element.className = 'hljs-' + element.className;
        }
        var numberOfElements = +element?.children.length;
        for (var i = 0; i < numberOfElements; i++) {
            setElements(element.children[i]);
        }
    }
}

export function fixEmoji() {
    var images = document.getElementsByClassName('emoji');
    var numberOfElements = images.length;
    for (var i = 0; i < numberOfElements; i++) {
        var image = images[i];
        if (image.tagName === 'IMG') {
            var src = image.getAttribute('data-src');
            if (src) {
                image.removeAttribute('data-src');
                image.setAttribute('src', src);
            }
        }
    }
}