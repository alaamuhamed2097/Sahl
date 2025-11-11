#!/usr/bin/env pwsh
# Quick Version Update Script
# This script is a shortcut for updating versions with common options

param(
    [ValidateSet("patch", "minor", "major")]
    [string]$Type = "patch",
    
    [switch]$Force,
    [switch]$Help
)

if ($Help) {
    Write-Host @"
?? Quick Version Update Script

Usage:
  .\quick-update.ps1 [options]

Options:
  -Type <patch|minor|major>  Version type to increment (default: patch)
  -Force                     Force update for all users
  -Help                      Show this help message

Examples:
  .\quick-update.ps1                    # Increment patch (1.0.0 ? 1.0.1)
  .\quick-update.ps1 -Type minor        # Increment minor (1.0.0 ? 1.1.0)
  .\quick-update.ps1 -Type major        # Increment major (1.0.0 ? 2.0.0)
  .\quick-update.ps1 -Force             # Force update with patch increment
  .\quick-update.ps1 -Type minor -Force # Force update with minor increment

"@
    exit 0
}

Write-Host "?? Quick Version Update" -ForegroundColor Cyan
Write-Host "Type: $Type" -ForegroundColor Gray
if ($Force) {
    Write-Host "Force: Yes" -ForegroundColor Yellow
}
Write-Host ""

# Call the main update script
& "$PSScriptRoot\update-version.ps1" -VersionType $Type -ForceUpdate $Force.IsPresent

Write-Host ""
Write-Host "? Done! Next steps:" -ForegroundColor Green
Write-Host "   1. Test: dotnet run" -ForegroundColor Gray
Write-Host "   2. Build: dotnet build" -ForegroundColor Gray
Write-Host "   3. Publish: dotnet publish -c Release" -ForegroundColor Gray
