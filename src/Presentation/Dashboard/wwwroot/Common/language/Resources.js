let _resources = {};

export function setResources(resources) {
    _resources = resources;
}

export function getString(key) {
    return _resources[key] || key; // Fallback to key if not found
}

export function formatString(key, ...args) {
    const template = _resources[key] || key;
    return template.replace(/{(\d+)}/g, (match, number) => {
        return typeof args[number] != 'undefined' ? args[number] : match;
    });
}