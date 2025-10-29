// localization.js
const localization = {
    setLanguage: function(languageCode) {
        localStorage.setItem('lang', languageCode.startsWith('ar') ? 'ar-EG' : 'en-US');
        this.initializeLanguage();
    },
    
    initializeLanguage: function() {
        const lang = localStorage.getItem('lang') || 'ar-EG';
        const isArabic = lang.startsWith('ar');
        
        // Set HTML attributes
        document.documentElement.lang = isArabic ? 'ar' : 'en';
        document.documentElement.dir = isArabic ? 'rtl' : 'ltr';
        
        // Manage RTL stylesheet
        if (isArabic) {
            this.loadRTLStylesheet();
        } else {
            this.removeRTLStylesheet();
        }
        
        // Dispatch event for other scripts to listen to
        document.dispatchEvent(new CustomEvent('languageChanged', { detail: lang }));
    },
    
    loadRTLStylesheet: function() {
        if (!document.getElementById('rtl-stylesheet')) {
            const link = document.createElement('link');
            link.id = 'rtl-stylesheet';
            link.rel = 'stylesheet';
            link.href = 'assets/css/layouts/rtl.css';
            document.head.appendChild(link);
        }
    },
    
    removeRTLStylesheet: function() {
        const rtlStylesheet = document.getElementById('rtl-stylesheet');
        if (rtlStylesheet) rtlStylesheet.remove();
    },
    
    getCurrentLanguage: function() {
        return localStorage.getItem('lang') || 'ar-EG';
    }
};

// Initialize on page load
localization.initializeLanguage();

// Make available globally
window.localization = localization;