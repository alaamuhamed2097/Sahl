# Add PDF system by reading buttons from file
$buttonsHtml = Get-Content "_pdf_buttons.html" -Raw -Encoding UTF8
$scriptTag = '    <script src="pdf-export.js"></script>'

$pages = Get-ChildItem page-*.html
$updated = 0

foreach ($page in $pages) {
    $content = Get-Content $page.FullName -Raw -Encoding UTF8
    $modified = $false
    
    # Add script
    if ($content -notmatch 'pdf-export\.js') {
        $content = $content.Replace('</head>', "$scriptTag`n</head>")
        $modified = $true
    }
    
    # Add buttons
    if ($content -notmatch 'pdf-buttons') {
        $pattern = '(<div class="navigation-header">[\s\S]*?</div>)'
        if ($content -match $pattern) {
            $navHeader = $matches[1]
            $replacement = $navHeader + "`n" + $buttonsHtml
            $content = $content.Replace($navHeader, $replacement)
            $modified = $true
        }
    }
    
    if ($modified) {
        $utf8 = New-Object System.Text.UTF8Encoding($false)
        [System.IO.File]::WriteAllText($page.FullName, $content, $utf8)
        Write-Host "Updated: $($page.Name)" -ForegroundColor Green
        $updated++
    }
}

Write-Host "`nTotal: $updated pages" -ForegroundColor Cyan
