export function getLanguageCode() {
    return document.documentElement.lang;
}

export function setLanguageCode(code) {
    document.documentElement.lang = code;
}
