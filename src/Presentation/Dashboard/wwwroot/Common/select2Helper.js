
window.initializeSelect2 = function (placeHolder) {
    try {
        $('.select2').select2({
            placeholder: placeHolder,
            allowClear: true,
            width: '100%',
            theme: 'bootstrap-5',
            language: {
                noResults: function () {
                    return document.documentElement.lang == "en" ? "No results found" : "لا توجد نتائج";
                },
                searching: function () {
                    return document.documentElement.lang == "en" ? "Searching..." : "جاري البحث...";
                }
            },
            escapeMarkup: function (markup) {
                return markup;
            }
        });

    } catch (error) {
        console.error('Error initializing Select2:', error);
    }
};