// select2Helper.js
// Place this file in wwwroot/js/Common/select2Helper.js
/**
 * Safely destroy Select2 instances
 * @param {string} selector - CSS selector for the select element(s)
 */
function safeDestroySelect2(selector) {
    try {
        $(selector).each(function () {
            const $element = $(this);
            // Check if Select2 is actually initialized on this element
            if ($element.hasClass('select2-hidden-accessible') && $element.data('select2')) {
                $element.select2('destroy');
            }
        });
    } catch (error) {
        console.warn('Select2 destroy warning:', error.message);
    }
}
/**
 * Initialize Select2 dropdowns for the category form
 * @param {Object} resources - Localization resources
 */
window.initializeSelect2 = function (resources) {
    try {
        // Safely destroy existing Select2 instances to prevent duplicates
        safeDestroySelect2('.select2-parent');
        safeDestroySelect2('.select2-attribute');

        // Initialize parent category select2
        $('.select2-parent').select2({
            placeholder: resources.parentCategoryPlaceholder || 'Select Parent Category',
            allowClear: true,
            width: '100%',
            theme: 'bootstrap-5',
            language: {
                noResults: function () {
                    return resources.noResults || "No results found";
                },
                searching: function () {
                    return resources.searching || "Searching...";
                }
            },
            escapeMarkup: function (markup) {
                return markup;
            }
        });

        // Initialize attribute select2 dropdowns
        $('.select2-attribute').each(function () {
            $(this).select2({
                placeholder: resources.attributePlaceholder || 'Select Attribute',
                allowClear: true,
                width: '100%',
                theme: 'bootstrap-5',
                language: {
                    noResults: function () {
                        return resources.noResults || "No results found";
                    },
                    searching: function () {
                        return resources.searching || "Searching...";
                    }
                },
                escapeMarkup: function (markup) {
                    return markup;
                }
            });
        });

        // Handle parent category change event
        $('.select2-parent').off('change.select2Category').on('change.select2Category', function (e) {
            var selectedValue = $(this).val();
            $(this).attr('value', selectedValue);
            // Trigger Blazor change event
            var event = new Event('change', { bubbles: true });
            this.dispatchEvent(event);
        });

        // Handle attribute selection change event
        $('.select2-attribute').off('change.select2Attribute').on('change.select2Attribute', function (e) {
            var selectedValue = $(this).val();
            $(this).attr('value', selectedValue);
            // Trigger Blazor change event
            var event = new Event('change', { bubbles: true });
            this.dispatchEvent(event);
        });

        console.log('Select2 initialized successfully');
    } catch (error) {
        console.error('Error initializing Select2:', error);
    }
};
/**
 * Update Select2 dropdown options dynamically
 * @param {string} selector - CSS selector for the select element
 * @param {Array} options - Array of {id, text} objects
 * @param {string} selectedValue - Currently selected value
 */
window.updateSelect2Options = function (selector, options, selectedValue) {
    try {
        var $select = $(selector);
        // Check if Select2 is initialized
        if (!$select.hasClass('select2-hidden-accessible')) {
            console.warn('Select2 not initialized on', selector);
            return;
        }
        // Clear existing options except the first placeholder option
        $select.find('option:not(:first)').remove();
        // Add new options
        options.forEach(function (option) {
            var newOption = new Option(option.text, option.id, false, option.id === selectedValue);
            $select.append(newOption);
        });
        // Refresh Select2 to show the new options
        $select.trigger('change');
    } catch (error) {
        console.error('Error updating Select2 options:', error);
    }
};
/**
 * Set selected value for Select2 dropdown
 * @param {string} selector - CSS selector for the select element
 * @param {string} value - Value to select
 */
window.setSelect2Value = function (selector, value) {
    try {
        const $element = $(selector);
        if ($element.hasClass('select2-hidden-accessible')) {
            $element.val(value).trigger('change');
        } else {
            console.warn('Select2 not initialized on', selector);
        }
    } catch (error) {
        console.error('Error setting Select2 value:', error);
    }
};
/**
 * Destroy Select2 instances safely
 * @param {string} selector - CSS selector for the select element(s)
 */
window.destroySelect2 = function (selector) {
    safeDestroySelect2(selector);
};
/**
 * Refresh Select2 dropdown after data changes
 * @param {string} selector - CSS selector for the select element
 */
window.refreshSelect2 = function (selector) {
    try {
        const $element = $(selector);
        // Only refresh if Select2 is initialized
        if ($element.hasClass('select2-hidden-accessible')) {
            $element.select2('destroy').select2({
                placeholder: 'Select an option',
                allowClear: true,
                width: '100%',
                theme: 'bootstrap-5'
            });
        } else {
            // Initialize if not already initialized
            $element.select2({
                placeholder: 'Select an option',
                allowClear: true,
                width: '100%',
                theme: 'bootstrap-5'
            });
        }
    } catch (error) {
        console.error('Error refreshing Select2:', error);
    }
};
/**
 * Check if Select2 is initialized on an element
 * @param {string} selector - CSS selector for the select element
 * @returns {boolean} - True if Select2 is initialized
 */
window.isSelect2Initialized = function (selector) {
    try {
        const $element = $(selector);
        return $element.hasClass('select2-hidden-accessible') && $element.data('select2') !== undefined;
    } catch (error) {
        console.error('Error checking Select2 initialization:', error);
        return false;
    }
};
/**
 * Set Select2 theme globally
 */
$(document).ready(function () {
    // Set default theme for all Select2 instances
    if (typeof $.fn.select2 !== 'undefined') {
        $.fn.select2.defaults.set('theme', 'bootstrap-5');
    }
});