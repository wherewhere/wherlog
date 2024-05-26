export function getValue(key) {
    return window.localStorage.getItem(key);
}

export function setValue(key, value) {
    window.localStorage.setItem(key, value);
}

export function clear() {
    window.localStorage.clear();
}
