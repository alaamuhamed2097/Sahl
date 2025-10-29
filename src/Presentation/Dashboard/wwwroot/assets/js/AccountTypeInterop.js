// Create this file at wwwroot/assets/js/AccountTypeInterop.js

window.Select2Helpers = {
    // Initialize Select2 with proper change event handling
    initializeSelect2: function (selector, placeholder, dotNetHelper) {
        $(selector).select2({
            placeholder: placeholder,
            allowClear: true,
            width: '100%'
        }).on('change', function () {
            if (dotNetHelper) {
                const selectedValues = $(selector).val() || [];
                dotNetHelper.invokeMethodAsync('OnSelect2Changed', selectedValues);
            }
        });
    },

    // Get selected values from Select2
    getSelectedSelect2Values: function (selector) {
        return $(selector).val() || [];
    },

    // Set selected values for Select2
    setSelect2Values: function (selector, values) {
        $(selector).val(values).trigger('change');
    },

    // Destroy Select2 instance
    destroySelect2: function (selector) {
        $(selector).off('change').select2('destroy');
    }
};
//window.Select2Helpers = {
//    // Initialize Select2 dropdown
//    initializeSelect2: function (selector, placeholder) {
//        $(selector).select2({
//            placeholder: placeholder,
//            width: '100%',
//            class: "form-control"
//        });
//    },

//    // Get selected values from Select2
//    getSelectedSelect2Values: function (selector) {
//        return $(selector).val(); // returns array of strings
//    },

//    // Set selected values programmatically (for edit mode)
//    setSelect2Values: function (selector, values) {
//        $(selector).val(values).trigger('change');
//    }
//};