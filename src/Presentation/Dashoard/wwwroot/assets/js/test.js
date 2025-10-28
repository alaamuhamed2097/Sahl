// أضف هذا الكود إلى ملف JavaScript الخاص بك
window.Select2Helpers = {
    // الطريقة الأصلية
    initializeSelect2: function (selector, placeholder) {
        $(selector).select2({
            placeholder: placeholder,
            allowClear: true,
            width: '100%'
        });
    },

    // طريقة محدثة مع callback
    initializeSelect2WithCallback: function (selector, placeholder, dotNetObjectRef, callbackMethodName) {
        // تدمير Select2 الموجود إن وجد
        this.destroySelect2(selector);

        const $element = $(selector);

        // التأكد من وجود العنصر
        if ($element.length === 0) {
            console.warn('Select2 element not found:', selector);
            return;
        }

        // تهيئة Select2
        $element.select2({
            placeholder: placeholder,
            allowClear: true,
            width: '100%'
        });

        // إزالة event listeners السابقة
        $element.off('change.select2custom');

        // إضافة event listener جديد للتغيير
        $element.on('change.select2custom', function (e) {
            const selectedValues = $(this).val() || [];
            console.log('Select2 values changed:', selectedValues);

            // استدعاء C# method
            if (dotNetObjectRef && callbackMethodName) {
                dotNetObjectRef.invokeMethodAsync(callbackMethodName, selectedValues)
                    .catch(function (error) {
                        console.error('Error calling .NET method:', error);
                    });
            }
        });

        console.log('Select2 initialized for:', selector);
    },

    // تحديد القيم المختارة
    setSelect2Values: function (selector, values) {
        const $element = $(selector);

        if ($element.length === 0) {
            console.warn('Select2 element not found for setting values:', selector);
            return;
        }

        // التأكد من أن Select2 مهيأ
        if (!$element.hasClass('select2-hidden-accessible')) {
            console.warn('Select2 not initialized for:', selector);
            return;
        }

        try {
            if (values && Array.isArray(values) && values.length > 0) {
                console.log('Setting Select2 values:', values);
                $element.val(values);
                // استخدام trigger('change.select2') بدلاً من trigger('change') لتجنب استدعاء callback
                $element.trigger('change.select2');
            } else {
                console.log('Clearing Select2 values');
                $element.val(null);
                $element.trigger('change.select2');
            }
        } catch (error) {
            console.error('Error setting Select2 values:', error);
        }
    },

    // الحصول على القيم المختارة
    getSelectedValues: function (selector) {
        const $element = $(selector);
        if ($element.length === 0 || !$element.hasClass('select2-hidden-accessible')) {
            return [];
        }
        return $element.val() || [];
    },

    // تدمير Select2
    destroySelect2: function (selector) {
        const $element = $(selector);
        if ($element.length > 0 && $element.hasClass('select2-hidden-accessible')) {
            try {
                $element.off('change.select2custom'); // إزالة event listeners المخصصة
                $element.select2('destroy');
                console.log('Select2 destroyed for:', selector);
            } catch (error) {
                console.error('Error destroying Select2:', error);
            }
        }
    },

    // فحص حالة Select2
    isSelect2Initialized: function (selector) {
        const $element = $(selector);
        return $element.length > 0 && $element.hasClass('select2-hidden-accessible');
    }
};
