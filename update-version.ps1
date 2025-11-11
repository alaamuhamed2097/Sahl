# Update Version Script
# This script updates the version.json file with a new version number
# Run this before deploying or during CI/CD pipeline

param(
    [string]$VersionType = "patch", # major, minor, patch
    [bool]$ForceUpdate = $false
)

$versionFilePath = Join-Path $PSScriptRoot "src\Presentation\Dashboard\wwwroot\version.json"

# Read current version
if (Test-Path $versionFilePath) {
    $versionData = Get-Content $versionFilePath -Raw | ConvertFrom-Json
    $currentVersion = $versionData.version
} else {
    Write-Host "? version.json not found at: $versionFilePath" -ForegroundColor Red
    exit 1
}

# Parse version
$versionParts = $currentVersion -split '\.'
[int]$major = $versionParts[0]
[int]$minor = $versionParts[1]
[int]$patch = $versionParts[2]

# Increment version based on type
switch ($VersionType.ToLower()) {
    "major" {
        $major++
        $minor = 0
        $patch = 0
    }
    "minor" {
        $minor++
        $patch = 0
    }
    "patch" {
        $patch++
    }
    default {
        $patch++
    }
}

$newVersion = "$major.$minor.$patch"

# Update version.json
$updatedData = @{
    version = $newVersion
    buildDate = (Get-Date).ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ")
    forceUpdate = $ForceUpdate
}

$updatedData | ConvertTo-Json -Depth 10 | Set-Content $versionFilePath -Encoding UTF8

Write-Host "? Version updated: $currentVersion ? $newVersion" -ForegroundColor Green
Write-Host "?? Build Date: $($updatedData.buildDate)" -ForegroundColor Cyan
Write-Host "?? Force Update: $ForceUpdate" -ForegroundColor $(if ($ForceUpdate) { "Yellow" } else { "Gray" })

# Update service-worker.js cache version
$serviceWorkerPath = Join-Path $PSScriptRoot "src\Presentation\Dashboard\wwwroot\service-worker.js"
if (Test-Path $serviceWorkerPath) {
    $serviceWorkerContent = Get-Content $serviceWorkerPath -Raw
    $serviceWorkerContent = $serviceWorkerContent -replace "const CACHE_VERSION = 'v[\d\.]+';", "const CACHE_VERSION = 'v$newVersion';"
    Set-Content $serviceWorkerPath -Value $serviceWorkerContent -Encoding UTF8
    Write-Host "? service-worker.js cache version updated" -ForegroundColor Green
}

# Update service-worker.published.js cache version
$serviceWorkerPublishedPath = Join-Path $PSScriptRoot "src\Presentation\Dashboard\wwwroot\service-worker.published.js"
if (Test-Path $serviceWorkerPublishedPath) {
    $serviceWorkerPublishedContent = Get-Content $serviceWorkerPublishedPath -Raw
    $serviceWorkerPublishedContent = $serviceWorkerPublishedContent -replace "const APP_VERSION = 'v[\d\.]+';", "const APP_VERSION = 'v$newVersion';"
    Set-Content $serviceWorkerPublishedPath -Value $serviceWorkerPublishedContent -Encoding UTF8
    Write-Host "? service-worker.published.js cache version updated" -ForegroundColor Green
}

Write-Host ""
Write-Host "?? Version update complete!" -ForegroundColor Green
Write-Host "?? New Version: $newVersion" -ForegroundColor White -BackgroundColor DarkGreen
Write-Host ""
Write-Host "?? Updated files:" -ForegroundColor Cyan
Write-Host "   • version.json" -ForegroundColor Gray
Write-Host "   • service-worker.js" -ForegroundColor Gray
Write-Host "   • service-worker.published.js" -ForegroundColor Gray
