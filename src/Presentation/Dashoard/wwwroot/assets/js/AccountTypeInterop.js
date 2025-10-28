window.Select2Helpers = {
    // Initialize Select2 with proper change event handling for flag enums
    initializeSelect2: function (selector, placeholder, dotNetHelper) {
        $(selector).select2({
            placeholder: placeholder,
            allowClear: true,
            width: '100%',
            closeOnSelect: false // Keep dropdown open for multi-select
        }).on('change', function (e) {
            if (dotNetHelper) {
                const selectedValues = $(selector).val() || [];
                // Invoke the C# method to handle the change
                dotNetHelper.invokeMethodAsync('OnSelect2Changed', selectedValues);
            }
        });
    },

    // Get selected values from Select2
    getSelectedSelect2Values: function (selector) {
        return $(selector).val() || [];
    },

    // Set selected values for Select2 (used in edit mode)
    setSelect2Values: function (selector, values) {
        if (values && values.length > 0) {
            $(selector).val(values).trigger('change');
        } else {
            $(selector).val(null).trigger('change');
        }
    },

    // Clear Select2 selection
    clearSelect2: function (selector) {
        $(selector).val(null).trigger('change');
    },

    // Destroy Select2 instance
    destroySelect2: function (selector) {
        if ($(selector).hasClass('select2-hidden-accessible')) {
            $(selector).off('change').select2('destroy');
        }
    },

    // Check if Select2 is initialized
    isSelect2Initialized: function (selector) {
        return $(selector).hasClass('select2-hidden-accessible');
    }
};