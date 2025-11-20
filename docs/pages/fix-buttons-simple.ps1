# Simple fix for PDF buttons placement
Get-ChildItem page-*.html | ForEach-Object {
    $content = [System.IO.File]::ReadAllText($_.FullName, [System.Text.Encoding]::UTF8)
    $original = $content
    
    # Replace the pattern
    $content = $content -replace '(?ms)(<span class="page-number">.*?</span>)\s*(<!-- أزرار تصدير PDF -->.*?<div class="pdf-buttons">.*?</div>)\s*</div>', '$1</div>' + [Environment]::NewLine + [Environment]::NewLine + '        $2'
    
    if ($content -ne $original) {
        [System.IO.File]::WriteAllText($_.FullName, $content, [System.Text.Encoding]::UTF8)
        Write-Host "Fixed: $($_.Name)" -ForegroundColor Green
    }
}
