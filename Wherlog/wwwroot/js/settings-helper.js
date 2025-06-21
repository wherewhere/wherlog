export function getValue(key) {
    const value = window.localStorage.getItem(key);
    if (typeof value === "undefined" || value === null) {
        return;
    }
    else {
        return value;
    }
}

export function setValue(key, value) {
    if (typeof value === "undefined" || value === null) {
        return;
    }
    else {
        window.localStorage.setItem(key, value);
    }
}

export function clear() {
    window.localStorage.clear();
}
