# Add PDF system with proper UTF-8 handling
Add-Type -AssemblyName System.Web

$pages = Get-ChildItem page-*.html
$updated = 0

foreach ($page in $pages) {
    # Read as bytes then decode as UTF-8
    $bytes = [System.IO.File]::ReadAllBytes($page.FullName)
    $content = [System.Text.Encoding]::UTF8.GetString($bytes)
    
    $modified = $false
    
    # Add script tag before </head>
    if ($content -notmatch 'pdf-export\.js') {
        $content = $content.Replace('</head>', "    <script src=`"pdf-export.js`"></script>`n</head>")
        $modified = $true
    }
    
    # Add PDF buttons after navigation-header closing div
    if ($content -notmatch 'pdf-buttons') {
        # Find the closing </div> after navigation-header
        $pattern = '(<div class="navigation-header">.*?</div>)'
        if ($content -match $pattern) {
            $navHeader = $matches[1]
            $replacement = $navHeader + "`n`n        <!-- أزرار تصدير PDF -->`n        <div class=`"pdf-buttons`">`n            <button onclick=`"exportCurrentPageToPDF()`" class=`"btn-pdf btn-pdf-single`">`n                📄 تصدير هذه الصفحة PDF`n            </button>`n            <button onclick=`"exportAllPagesToPDF()`" class=`"btn-pdf btn-pdf-all`">`n                📚 تصدير جميع الصفحات PDF`n            </button>`n        </div>"
            $content = $content.Replace($navHeader, $replacement)
            $modified = $true
        }
    }
    
    if ($modified) {
        # Write back as UTF-8 without BOM
        $utf8 = New-Object System.Text.UTF8Encoding($false)
        $bytes = $utf8.GetBytes($content)
        [System.IO.File]::WriteAllBytes($page.FullName, $bytes)
        Write-Host "Updated: $($page.Name)" -ForegroundColor Green
        $updated++
    }
}

Write-Host "`nTotal: $updated pages" -ForegroundColor Cyan
