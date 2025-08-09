export function getValue(key) {
    return localStorage.getItem(key);
}

export function setValue(key, value) {
    localStorage.setItem(key, value);
}

export function clear() {
    localStorage.clear();
}
