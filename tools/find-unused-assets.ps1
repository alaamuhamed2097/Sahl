# PowerShell script to find unused files under wwwroot/assets
# Usage: powershell -NoProfile -ExecutionPolicy Bypass -File tools\find-unused-assets.ps1

$assetsDir = "src\Presentation\Dashboard\wwwroot\assets"
if (-not (Test-Path $assetsDir)) {
 Write-Error "Assets directory not found: $assetsDir"
 exit1
}

Write-Output "Scanning assets in: $assetsDir"

# Define text file extensions to search inside
$searchExtensions = @('.html', '.htm', '.cshtml', '.razor', '.js', '.ts', '.css', '.scss', '.less', '.cs', '.json', '.xml', '.config', '.txt', '.md')

# Gather all asset files (exclude source maps and some binary types)
$assetFiles = Get-ChildItem -Path $assetsDir -Recurse -File | Where-Object { $_.Extension -notin @('.map', '.png', '.jpg', '.jpeg', '.gif', '.webp', '.svg') }

# Prepare the files to search in - exclude the assets folder itself and common binary/build folders
$repoFiles = Get-ChildItem -Path . -Recurse -File | Where-Object {
 ($_.FullName -notmatch [regex]::Escape((Get-Item $assetsDir).FullName)) -and
 ($_.FullName -notmatch '\\bin\\') -and
 ($_.FullName -notmatch '\\obj\\') -and
 ($_.FullName -notmatch '\\node_modules\\') -and
 ($_.Extension -in $searchExtensions)
}

if ($repoFiles.Count -eq0) {
 Write-Warning "No searchable files found in repository."
}

$unused = @()

foreach ($file in $assetFiles) {
 $fileName = $file.Name

 # If file is e.g. 'font.woff' it might be referenced by URL without the extension in CSS — we still search by filename
 $pattern = [regex]::Escape($fileName)

 # Search in the repo files for the filename
 $found = $false
 foreach ($searchFile in $repoFiles) {
 try {
 if (Select-String -Path $searchFile.FullName -Pattern $pattern -SimpleMatch -Quiet) {
 $found = $true
 break
 }
 } catch {
 # Ignore files that cannot be read
 }
 }

 if (-not $found) {
 $unused += $file
 Write-Output "UNUSED: $($file.FullName)"
 }
}

# Save report
$reportPath = 'unused-assets-report.txt'
$unused | ForEach-Object { $_.FullName } | Out-File -FilePath $reportPath -Encoding UTF8

Write-Output "Scan complete. Unused files: $($unused.Count)"
Write-Output "Report saved to: $reportPath"
