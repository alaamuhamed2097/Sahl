/**
 * PDF Export System for Documentation Pages
 * Handles single page and batch PDF export functionality
 */

// Export current page to PDF
function exportCurrentPageToPDF() {
    // Hide navigation elements temporarily
    const header = document.querySelector('.navigation-header');
    const footer = document.querySelector('.navigation-footer');
    const pdfButtons = document.querySelector('.pdf-buttons');
    
    if (header) header.style.display = 'none';
    if (footer) footer.style.display = 'none';
    if (pdfButtons) pdfButtons.style.display = 'none';
    
    // Trigger print dialog
    window.print();
    
    // Restore navigation elements after print
    setTimeout(() => {
        if (header) header.style.display = 'flex';
        if (footer) footer.style.display = 'flex';
        if (pdfButtons) pdfButtons.style.display = 'flex';
    }, 100);
}

// Export all pages to PDF (batch export)
async function exportAllPagesToPDF() {
    const pageFiles = [
        'page-01.html',
        'page-02.html',
        'page-03a.html',
        'page-03b.html',
        'page-03c.html',
        'page-04.html',
        'page-05.html',
        'page-06.html',
        'page-07.html',
        'page-08.html',
        'page-09.html',
        'page-10.html',
        'page-11.html',
        'page-12.html',
        'page-13.html',
        'page-14.html',
        'page-15.html',
        'page-16.html',
        'page-17.html',
        'page-18.html',
        'page-19.html',
        'page-20.html',
        'page-21.html',
        'page-22.html',
        'page-23.html',
        'page-24.html',
        'page-25.html',
        'page-26.html',
        'page-27.html',
        'page-28.html',
        'page-29.html',
        'page-30.html',
        'page-31.html'
    ];
    
    alert('سيتم فتح نافذة طباعة لكل صفحة. يرجى اختيار "حفظ كـ PDF" من كل نافذة.\n\nعدد الصفحات: ' + pageFiles.length);
    
    for (let i = 0; i < pageFiles.length; i++) {
        const pageFile = pageFiles[i];
        const pageWindow = window.open(pageFile, '_blank');
        
        if (pageWindow) {
            // Wait for page to load
            await new Promise(resolve => {
                pageWindow.addEventListener('load', () => {
                    setTimeout(() => {
                        // Hide navigation elements
                        const header = pageWindow.document.querySelector('.navigation-header');
                        const footer = pageWindow.document.querySelector('.navigation-footer');
                        const pdfButtons = pageWindow.document.querySelector('.pdf-buttons');
                        
                        if (header) header.style.display = 'none';
                        if (footer) footer.style.display = 'none';
                        if (pdfButtons) pdfButtons.style.display = 'none';
                        
                        // Trigger print
                        pageWindow.print();
                        
                        // Close window after print
                        setTimeout(() => {
                            pageWindow.close();
                            resolve();
                        }, 500);
                    }, 500);
                });
            });
        }
        
        // Small delay between pages
        await new Promise(resolve => setTimeout(resolve, 1000));
    }
    
    alert('اكتمل تصدير جميع الصفحات!');
}

// Initialize PDF export buttons when page loads
document.addEventListener('DOMContentLoaded', function() {
    // Check if PDF buttons container exists
    const pdfButtonsContainer = document.querySelector('.pdf-buttons');
    
    if (pdfButtonsContainer) {
        // Try to find buttons by ID first (new pages)
        let currentPageBtn = document.getElementById('exportCurrentPage');
        let allPagesBtn = document.getElementById('exportAllPages');
        
        // Fallback to class selectors (if using different approach)
        if (!currentPageBtn) {
            currentPageBtn = pdfButtonsContainer.querySelector('.btn-pdf-single');
        }
        if (!allPagesBtn) {
            allPagesBtn = pdfButtonsContainer.querySelector('.btn-pdf-all');
        }
        
        if (currentPageBtn) {
            currentPageBtn.addEventListener('click', exportCurrentPageToPDF);
        }
        
        if (allPagesBtn) {
            allPagesBtn.addEventListener('click', exportAllPagesToPDF);
        }
    }
});
