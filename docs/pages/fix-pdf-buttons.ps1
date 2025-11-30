# Fix misplaced PDF buttons in HTML files
$pages = Get-ChildItem page-*.html
$fixed = 0

foreach ($page in $pages) {
    $content = Get-Content $page.Name -Raw -Encoding UTF8
    $originalContent = $content
    
    # Find the pattern and fix it
    $lines = $content -split "`r?`n"
    $newLines = @()
    $i = 0
    $inFix = $false
    
    while ($i -lt $lines.Count) {
        $line = $lines[$i]
        
        # Check if this line has page-number and next has PDF buttons comment
        if ($line -match 'page-number.*</span>\s*$' -and $i+1 -lt $lines.Count -and $lines[$i+1] -match '^\s*<!-- أزرار تصدير PDF -->') {
            # Add closing div after page-number
            $newLines += $line
            $newLines += '        </div>'
            $newLines += ''
            # Continue adding PDF button lines
            $i++
            $inFix = $true
        }
        elseif ($inFix -and $line -match '^\s*</div>\s*$' -and $lines[$i-1] -match '</div>') {
            # Skip this extra closing div
            $inFix = $false
            $i++
            continue
        }
        else {
            $newLines += $line
        }
        $i++
    }
    
    $content = $newLines -join "`r`n"
    
    if ($content -ne $originalContent) {
        Set-Content $page.Name -Value $content -NoNewline -Encoding UTF8
        Write-Host "Fixed: $($page.Name)" -ForegroundColor Yellow
        $fixed++
    }
}

Write-Host "`nTotal fixed: $fixed pages" -ForegroundColor Green
