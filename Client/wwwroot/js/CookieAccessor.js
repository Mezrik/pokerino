export function get(key) {
    const value = `; ${document.cookie}`;
    const parts = value.split(`; ${name}=`);
    if (parts.length === 2) return parts.pop().split(';').shift();
}

export function set(key, value) {
    document.cookie = `${key}=${value}`;
}

export function remove(key) {
    document.cookie = `${name}=; Max-Age=-99999999;`;
}