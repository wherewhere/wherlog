export function getAndRemoveExistingMeta(name) {
    const meta = document.querySelector(`meta[name="${name}"]`);
    if (meta instanceof HTMLMetaElement) {
        meta.remove();
        return meta.content;
    }
}

export function getAndRemoveExistingThemeColor() {
    const colors = document.querySelectorAll('meta[name="theme-color"]');
    const array = [];
    colors.forEach(x => {
        if (x instanceof HTMLMetaElement) {
            x.remove();
            array.push({ content: x.content, media: x.media });
        }
    });
    return array;
}