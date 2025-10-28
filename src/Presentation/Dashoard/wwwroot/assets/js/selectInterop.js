window.Select2Helpers = {
    initializeSelect2: function (selector, placeholder, dotNetRef, methodName) {
        // Destroy existing Select2 if any
        if ($(selector).hasClass('select2-hidden-accessible')) {
            $(selector).select2('destroy');
        }

        // Initialize Select2 with callback to Blazor
        $(selector).select2({
            placeholder: placeholder,
            width: '100%'
        }).on('change', function (e) {
            // Get selected values and call Blazor method
            const selectedValues = $(this).val() || [];
            console.log('Select2 changed, selected values:', selectedValues);

            // Call the Blazor method
            dotNetRef.invokeMethodAsync(methodName, selectedValues)
                .catch(error => {
                    console.error('Error calling Blazor method:', error);
                });
        });

        console.log('Select2 initialized for:', selector);
    },

    getSelectedValues: function (selector) {
        return $(selector).val() || [];
    },

    setSelect2Values: function (selector, values) {
        console.log('Setting Select2 values:', values);
        $(selector).val(values).trigger('change');
    },

    destroySelect2: function (selector) {
        if ($(selector).hasClass('select2-hidden-accessible')) {
            $(selector).select2('destroy');
        }
    }
};