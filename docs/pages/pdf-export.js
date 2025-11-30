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

// Export all pages to PDF (batch export) - Sequential printing
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
    
    const confirmed = confirm(
        `سيتم فتح جميع الصفحات (${pageFiles.length} صفحة) في نافذة واحدة للطباعة.\n\n` +
        `في نافذة الطباعة:\n` +
        `1. اختر "Microsoft Print to PDF" أو "Save as PDF"\n` +
        `2. اضغط Print/Save\n\n` +
        `هل تريد المتابعة؟`
    );
    
    if (!confirmed) {
        return;
    }
    
    // Create a new window with all pages
    const combinedWindow = window.open('', '_blank', 'width=1200,height=800');
    
    if (!combinedWindow) {
        alert('تم حظر النافذة المنبثقة! يرجى السماح بالنوافذ المنبثقة من إعدادات المتصفح وإعادة المحاولة.');
        return;
    }
    
    // Write initial HTML structure
    combinedWindow.document.write(`
        <!DOCTYPE html>
        <html lang="ar" dir="rtl">
        <head>
            <meta charset="UTF-8">
            <meta name="viewport" content="width=device-width, initial-scale=1.0">
            <title>وثيقة متطلبات العمل - كاملة</title>
            <link rel="preconnect" href="https://fonts.googleapis.com">
            <link rel="preconnect" href="https://fonts.gstatic.com" crossorigin>
            <link href="https://fonts.googleapis.com/css2?family=Cairo:wght@400;700;900&display=swap" rel="stylesheet">
            <link rel="stylesheet" href="style.css">
            <style>
                @media print {
                    .page-content { page-break-after: always; }
                    .loading-msg { display: none !important; }
                }
                body { 
                    background: white; 
                    padding: 20px;
                }
                .loading-msg { 
                    text-align: center; 
                    padding: 50px; 
                    font-size: 1.5em; 
                    color: #667eea;
                    font-weight: bold;
                }
                .page-content {
                    margin-bottom: 40px;
                    height: auto;
                }
                iframe {
                    width: 100%;
                    min-height: 800px;
                    border: 2px solid #e0e0e0;
                    margin-bottom: 20px;
                    border-radius: 8px;
                }
            </style>
        </head>
        <body>
            <div class="loading-msg">⏳ جاري تحميل الصفحات... الرجاء الانتظار</div>
            <div id="pages-container"></div>
        </body>
        </html>
    `);
    
    const container = combinedWindow.document.getElementById('pages-container');
    const loadingMsg = combinedWindow.document.querySelector('.loading-msg');
    
    let loadedCount = 0;
    
    // Load all pages in iframes
    for (let i = 0; i < pageFiles.length; i++) {
        const iframe = combinedWindow.document.createElement('iframe');
        iframe.src = pageFiles[i];
        iframe.className = 'page-content';
        
        iframe.onload = function() {
            loadedCount++;
            loadingMsg.textContent = `⏳ تم تحميل ${loadedCount} من ${pageFiles.length} صفحة...`;
            
            // When all pages are loaded
            if (loadedCount === pageFiles.length) {
                loadingMsg.textContent = '✅ تم تحميل جميع الصفحات! جاري فتح نافذة الطباعة...';
                
                setTimeout(() => {
                    loadingMsg.style.display = 'none';
                    alert('✅ جاهز للطباعة!\n\nاختر "Save as PDF" أو "Microsoft Print to PDF" من نافذة الطباعة التالية.');
                    combinedWindow.print();
                }, 1000);
            }
        };
        
        container.appendChild(iframe);
    }
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
