# Clear Cache Script for Blazor WASM
# ====================================

Write-Host "?? Clearing Blazor WASM Cache..." -ForegroundColor Cyan
Write-Host ""

# 1. Stop any running dotnet processes
Write-Host "1?? Stopping running processes..." -ForegroundColor Yellow
Get-Process -Name "dotnet" -ErrorAction SilentlyContinue | Stop-Process -Force -ErrorAction SilentlyContinue
Start-Sleep -Seconds 2

# 2. Clean bin and obj folders
Write-Host "2?? Cleaning bin and obj folders..." -ForegroundColor Yellow
Remove-Item -Path "bin" -Recurse -Force -ErrorAction SilentlyContinue
Remove-Item -Path "obj" -Recurse -Force -ErrorAction SilentlyContinue

# 3. Run dotnet clean
Write-Host "3?? Running dotnet clean..." -ForegroundColor Yellow
dotnet clean --nologo | Out-Null

# 4. Clear NuGet cache (optional, uncomment if needed)
# Write-Host "4?? Clearing NuGet cache..." -ForegroundColor Yellow
# dotnet nuget locals all --clear

Write-Host ""
Write-Host "? Cache cleared successfully!" -ForegroundColor Green
Write-Host ""
Write-Host "?? Next steps:" -ForegroundColor Cyan
Write-Host "   1. Run: dotnet build" -ForegroundColor Gray
Write-Host "   2. Run: dotnet run" -ForegroundColor Gray
Write-Host "   3. Open browser in incognito mode" -ForegroundColor Gray
Write-Host "   4. Press Ctrl+Shift+R to hard refresh" -ForegroundColor Gray
Write-Host ""
