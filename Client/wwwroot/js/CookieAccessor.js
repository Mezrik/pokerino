const converter = {
    read: function (value) {
        if (value[0] === '"') {
            value = value.slice(1, -1)
        }
        return value.replace(/(%[\dA-F]{2})+/gi, decodeURIComponent)
    },
    write: function (value) {
        return encodeURIComponent(value).replace(
            /%(2[346BF]|3[AC-F]|40|5[BDE]|60|7[BCD])/g,
            decodeURIComponent
        )
    }
}

export function get(key) {
    const cookies = document.cookie ? document.cookie.split('; ') : []
    const jar = {}
    for (let i = 0; i < cookies.length; i++) {
        const parts = cookies[i].split('=')
        const value = parts.slice(1).join('=')

        try {
            const found = decodeURIComponent(parts[0])
            jar[found] = converter.read(value, found)

            if (key === found) {
                break
            }
        } catch (e) { }
    }

    return key ? jar[key] : jar
}

export function set(key, value) {
    document.cookie = `${key}=${value}`;
}

export function remove(key) {
    document.cookie = `${key}=; Max-Age=-99999999;`;
}