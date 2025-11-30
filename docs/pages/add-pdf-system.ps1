# Add PDF export system to all pages - UTF8 safe
$scriptTag = '    <script src="pdf-export.js"></script>'
$pdfButtons = @'

        <!-- أزرار تصدير PDF -->
        <div class="pdf-buttons">
            <button onclick="exportCurrentPageToPDF()" class="btn-pdf btn-pdf-single">
                📄 تصدير هذه الصفحة PDF
            </button>
            <button onclick="exportAllPagesToPDF()" class="btn-pdf btn-pdf-all">
                📚 تصدير جميع الصفحات PDF
            </button>
        </div>
'@

$pages = Get-ChildItem page-*.html
$added = 0

foreach ($page in $pages) {
    # Read with UTF8
    $content = [System.IO.File]::ReadAllText($page.FullName, [System.Text.Encoding]::UTF8)
    $modified = $false
    
    # Add script if not exists
    if ($content -notmatch 'pdf-export\.js') {
        $content = $content -replace '</head>', "$scriptTag`n</head>"
        $modified = $true
    }
    
    # Add buttons if not exists
    if ($content -notmatch 'pdf-buttons') {
        $content = $content -replace '(</div>\s*<div class="content">)', "$pdfButtons`n`$1"
        $modified = $true
    }
    
    if ($modified) {
        # Write with UTF8 (no BOM)
        $utf8NoBom = New-Object System.Text.UTF8Encoding $false
        [System.IO.File]::WriteAllText($page.FullName, $content, $utf8NoBom)
        Write-Host "Updated: $($page.Name)" -ForegroundColor Green
        $added++
    }
}

Write-Host "`nTotal updated: $added pages" -ForegroundColor Cyan
