window.RepeaterHelper = {
    /**
     * Initialize the repeater with Blazor compatibility
     */
    initRepeater: function () {
        try {
            if (typeof jQuery === 'undefined') {
                console.error("jQuery is not loaded");
                return false;
            }

            const $repeater = $('.repeater');
            if ($repeater.length === 0) {
                console.error("Repeater element not found");
                return false;
            }

            $repeater.repeater({
                initEmpty: false,
                show: function () {
                    $(this).slideDown();
                },
                hide: function (deleteElement) {
                    swal({
                        title: ValidationResources.ConfirmPrompt,
                        text: ValidationResources.DeleteWarning,
                        icon: "warning",
                        buttons: {
                            cancel: ActionsResources.Cancel,
                            confirm: ActionsResources.Confirm
                        },
                        dangerMode: true
                    }).then((willDelete) => {
                        if (willDelete) {
                            $(this).slideUp(deleteElement);
                        }
                    });
                },
                ready: function () {
                    console.log("Repeater initialized");
                },
                isFirstItemUndeletable: false
            });

            return true;
        } catch (error) {
            console.error("Repeater initialization error:", error);
            return false;
        }
    },

    /**
     * Get repeater values in Blazor-compatible format
     */
    getRepeaterValues: function () {
        try {
            const $repeater = $('.repeater');
            if ($repeater.length === 0) return null;

            const values = [];
            $repeater.find('[data-repeater-item]').each(function () {
                const $item = $(this);
                values.push({
                    AttributeId: $item.find('select').val(),
                    IsRequired: $item.find('#attrIsRequired').is(':checked'),
                    AffectsPricing: $item.find('#attrIsAffectPrice').is(':checked')
                });
            });
            return values;
        } catch (error) {
            console.error("Error getting repeater values:", error);
            return null;
        }
    },

    /**
     * Set repeater values
     */
    setRepeaterValues: function (values) {
        try {
            const $repeater = $('.repeater');
            if ($repeater.length === 0) return false;

            // First reset the repeater
            $repeater.repeater('remove');

            // Add items with values
            if (values && values.length > 0) {
                values.forEach(item => {
                    const $newItem = $repeater.repeater('add');
                    $newItem.find('select').val(item.attributeId);
                    $newItem.find('#attrIsRequired').prop('checked', item.isRequired);
                    $newItem.find('#attrIsAffectPrice').prop('checked', item.isAffectPrice);
                });
            }
            return true;
        } catch (error) {
            console.error("Error setting repeater values:", error);
            return false;
        }
    }
};                                 