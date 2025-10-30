// Local Storage Helper for Blazor WebAssembly
// Provides utility functions for localStorage operations

window.localStorageHelper = window.localStorageHelper || {};

// Get auto-load preference (used for statistics dashboard)
window.getAutoLoadPreference = function () {
    try {
        const value = localStorage.getItem('autoLoadPreference');
        return value === 'true' || value === null; // Default to true if not set
    } catch (e) {
        console.error('Error reading autoLoadPreference from localStorage:', e);
        return true; // Default to true on error
    }
};

// Set auto-load preference
window.setAutoLoadPreference = function (value) {
    try {
        localStorage.setItem('autoLoadPreference', value.toString());
        return true;
    } catch (e) {
        console.error('Error writing autoLoadPreference to localStorage:', e);
        return false;
    }
};

// Generic localStorage helper functions
window.localStorageHelper = {
    // Get an item from localStorage
    getItem: function (key) {
        try {
            return localStorage.getItem(key);
        } catch (e) {
            console.error(`Error reading ${key} from localStorage:`, e);
            return null;
        }
    },

    // Set an item in localStorage
    setItem: function (key, value) {
        try {
            localStorage.setItem(key, value);
            return true;
        } catch (e) {
            console.error(`Error writing ${key} to localStorage:`, e);
            return false;
        }
    },

    // Remove an item from localStorage
    removeItem: function (key) {
        try {
            localStorage.removeItem(key);
            return true;
        } catch (e) {
            console.error(`Error removing ${key} from localStorage:`, e);
            return false;
        }
    },

    // Clear all localStorage
    clear: function () {
        try {
            localStorage.clear();
            return true;
        } catch (e) {
            console.error('Error clearing localStorage:', e);
            return false;
        }
    },

    // Check if a key exists in localStorage
    hasKey: function (key) {
        try {
            return localStorage.getItem(key) !== null;
        } catch (e) {
            console.error(`Error checking ${key} in localStorage:`, e);
            return false;
        }
    },

    // Get item as JSON
    getItemAsJson: function (key) {
        try {
            const item = localStorage.getItem(key);
            return item ? JSON.parse(item) : null;
        } catch (e) {
            console.error(`Error parsing ${key} from localStorage:`, e);
            return null;
        }
    },

    // Set item as JSON
    setItemAsJson: function (key, value) {
        try {
            localStorage.setItem(key, JSON.stringify(value));
            return true;
        } catch (e) {
            console.error(`Error writing ${key} to localStorage as JSON:`, e);
            return false;
        }
    }
};

// Export functions for module systems if needed
if (typeof module !== 'undefined' && module.exports) {
    module.exports = {
        getAutoLoadPreference: window.getAutoLoadPreference,
        setAutoLoadPreference: window.setAutoLoadPreference,
        localStorageHelper: window.localStorageHelper
    };
}
