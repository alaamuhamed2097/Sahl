# Auto Migration Script - Copy Versioning System to Another Project
# Usage: .\migrate-to-project.ps1 -TargetProject "D:\Path\To\NewProject" -BlazorPath "src\Web"

param(
    [Parameter(Mandatory=$true)]
    [string]$TargetProject,
    
    [Parameter(Mandatory=$true)]
    [string]$BlazorPath,
    
    [switch]$IncludeDocumentation
)

Write-Host @"
????????????????????????????????????????????????????????????
?     ?? ???? ??? Versioning ?????? ???                  ?
?     Auto Migration Script                               ?
????????????????????????????????????????????????????????????
"@ -ForegroundColor Cyan

Write-Host ""

# Validate paths
if (!(Test-Path $TargetProject)) {
    Write-Host "? ?????? ???????? ??? ?????: $TargetProject" -ForegroundColor Red
    exit 1
}

$currentProject = $PSScriptRoot
$targetWwwroot = Join-Path $TargetProject "$BlazorPath\wwwroot"

if (!(Test-Path $targetWwwroot)) {
    Write-Host "?? wwwroot ??? ????? ??: $targetWwwroot" -ForegroundColor Yellow
    Write-Host "?? ???? ??????? (Y/N)" -ForegroundColor Yellow
    $create = Read-Host
    if ($create -eq "Y" -or $create -eq "y") {
        New-Item -Path $targetWwwroot -ItemType Directory -Force | Out-Null
        Write-Host "? ?? ????? ??????" -ForegroundColor Green
    } else {
        Write-Host "? ?? ???????" -ForegroundColor Red
        exit 1
    }
}

Write-Host "?? ??????: $currentProject" -ForegroundColor Gray
Write-Host "?? ?????: $TargetProject" -ForegroundColor Gray
Write-Host "?? Blazor: $BlazorPath" -ForegroundColor Gray
Write-Host ""

# Start migration
Write-Host "?? ??? ?????..." -ForegroundColor Cyan
Write-Host ""

# Step 1: Copy scripts
Write-Host "1?? ??? Scripts..." -ForegroundColor Yellow
try {
    Copy-Item "$currentProject\quick-update.bat" $TargetProject -Force
    Write-Host "   ? quick-update.bat" -ForegroundColor Green
    
    Copy-Item "$currentProject\quick-update.ps1" $TargetProject -Force
    Write-Host "   ? quick-update.ps1" -ForegroundColor Green
    
    Copy-Item "$currentProject\update-version.ps1" $TargetProject -Force
    Write-Host "   ? update-version.ps1" -ForegroundColor Green
    
    # Update paths in update-version.ps1
    $updateScriptPath = Join-Path $TargetProject "update-version.ps1"
    $content = Get-Content $updateScriptPath -Raw
    
    # Replace Dashboard path with new BlazorPath
    $content = $content -replace 'src\\Presentation\\Dashboard', $BlazorPath.Replace('/', '\')
    
    Set-Content $updateScriptPath -Value $content -Encoding UTF8
    Write-Host "   ? ?? ????? ???????? ?? update-version.ps1" -ForegroundColor Green
    
} catch {
    Write-Host "   ? ??? ??? Scripts: $_" -ForegroundColor Red
    exit 1
}

Write-Host ""

# Step 2: Copy wwwroot files
Write-Host "2?? ??? ????? wwwroot..." -ForegroundColor Yellow
try {
    # version.json
    Copy-Item "$currentProject\src\Presentation\Dashboard\wwwroot\version.json" $targetWwwroot -Force
    Write-Host "   ? version.json" -ForegroundColor Green
    
    # service-worker.js
    Copy-Item "$currentProject\src\Presentation\Dashboard\wwwroot\service-worker.js" $targetWwwroot -Force
    Write-Host "   ? service-worker.js" -ForegroundColor Green
    
    # service-worker.published.js
    Copy-Item "$currentProject\src\Presentation\Dashboard\wwwroot\service-worker.published.js" $targetWwwroot -Force
    Write-Host "   ? service-worker.published.js" -ForegroundColor Green
    
    # Create js folder if not exists
    $jsFolder = Join-Path $targetWwwroot "js"
    if (!(Test-Path $jsFolder)) {
        New-Item -Path $jsFolder -ItemType Directory -Force | Out-Null
        Write-Host "   ? ?? ????? ???? js" -ForegroundColor Green
    }
    
    # version-manager.js
    Copy-Item "$currentProject\src\Presentation\Dashboard\wwwroot\js\version-manager.js" $jsFolder -Force
    Write-Host "   ? version-manager.js" -ForegroundColor Green
    
} catch {
    Write-Host "   ? ??? ??? wwwroot files: $_" -ForegroundColor Red
    exit 1
}

Write-Host ""

# Step 3: Copy .vscode (optional)
if (Test-Path "$currentProject\.vscode\launch.json") {
    Write-Host "3?? ??? .vscode..." -ForegroundColor Yellow
    try {
        $vscodePath = Join-Path $TargetProject ".vscode"
        if (!(Test-Path $vscodePath)) {
            New-Item -Path $vscodePath -ItemType Directory -Force | Out-Null
        }
        
        Copy-Item "$currentProject\.vscode\launch.json" $vscodePath -Force
        Write-Host "   ? launch.json" -ForegroundColor Green
    } catch {
        Write-Host "   ?? ?????: ??? ??? .vscode: $_" -ForegroundColor Yellow
    }
    Write-Host ""
}

# Step 4: Copy documentation (if requested)
if ($IncludeDocumentation) {
    Write-Host "4?? ??? ???????..." -ForegroundColor Yellow
    try {
        $docs = @(
            "DEPLOYMENT_GUIDE.md",
            "DEPLOYMENT_QUICK.md",
            "SERVICE_WORKERS_GUIDE.md",
            "QUICK_REFERENCE.md",
            "README_VERSION_SYSTEM.md",
            "MIGRATION_TO_OTHER_PROJECT.md"
        )
        
        foreach ($doc in $docs) {
            if (Test-Path "$currentProject\$doc") {
                Copy-Item "$currentProject\$doc" $TargetProject -Force
                Write-Host "   ? $doc" -ForegroundColor Green
            }
        }
    } catch {
        Write-Host "   ?? ?????: ??? ??? ??? ????? ???????: $_" -ForegroundColor Yellow
    }
    Write-Host ""
}

# Step 5: Check index.html
Write-Host "5?? ??? index.html..." -ForegroundColor Yellow
$indexPath = Join-Path $targetWwwroot "index.html"

if (Test-Path $indexPath) {
    $indexContent = Get-Content $indexPath -Raw
    
    if ($indexContent -match 'version-manager\.js') {
        Write-Host "   ? version-manager.js ????? ?????? ?? index.html" -ForegroundColor Green
    } else {
        Write-Host "   ?? version-manager.js ??? ????? ?? index.html" -ForegroundColor Yellow
        Write-Host "   ?? ???? ?????? ????????? (Y/N)" -ForegroundColor Yellow
        $add = Read-Host
        
        if ($add -eq "Y" -or $add -eq "y") {
            # Add before </body>
            $indexContent = $indexContent -replace '</body>', "    <script src=`"js/version-manager.js`"></script>`n</body>"
            Set-Content $indexPath -Value $indexContent -Encoding UTF8
            Write-Host "   ? ??? ???????" -ForegroundColor Green
        } else {
            Write-Host "   ?? ??? ?????? ??????:" -ForegroundColor Cyan
            Write-Host '   <script src="js/version-manager.js"></script>' -ForegroundColor Gray
            Write-Host "   ??? </body> ?? index.html" -ForegroundColor Gray
        }
    }
} else {
    Write-Host "   ?? index.html ??? ????? ?? $targetWwwroot" -ForegroundColor Yellow
}

Write-Host ""

# Summary
Write-Host "????????????????????????????????????????????????????????????" -ForegroundColor Green
Write-Host "?     ? ?? ????? ?????!                                  ?" -ForegroundColor Green
Write-Host "????????????????????????????????????????????????????????????" -ForegroundColor Green
Write-Host ""

Write-Host "?? ??????? ????????:" -ForegroundColor Cyan
Write-Host "   • quick-update.bat" -ForegroundColor Gray
Write-Host "   • quick-update.ps1" -ForegroundColor Gray
Write-Host "   • update-version.ps1" -ForegroundColor Gray
Write-Host "   • version.json" -ForegroundColor Gray
Write-Host "   • service-worker.js" -ForegroundColor Gray
Write-Host "   • service-worker.published.js" -ForegroundColor Gray
Write-Host "   • version-manager.js" -ForegroundColor Gray

if ($IncludeDocumentation) {
    Write-Host "   • Documentation files" -ForegroundColor Gray
}

Write-Host ""
Write-Host "?? ??????? ???????:" -ForegroundColor Cyan
Write-Host "   1. ???? ?? index.html (??? ?? ????? ??? version-manager.js)" -ForegroundColor Yellow
Write-Host "   2. ????: .\quick-update.bat" -ForegroundColor Yellow
Write-Host "   3. ?????: dotnet run" -ForegroundColor Yellow
Write-Host "   4. ???? ?? Console (F12)" -ForegroundColor Yellow
Write-Host ""

Write-Host "?? ???????:" -ForegroundColor Cyan
if ($IncludeDocumentation) {
    Write-Host "   ????: MIGRATION_TO_OTHER_PROJECT.md" -ForegroundColor Gray
} else {
    Write-Host "   ?? ?????: ???? ???????? ?? -IncludeDocumentation ???? ???????" -ForegroundColor Gray
}

Write-Host ""
Write-Host "?? ?????? ???????? ???? Versioning ?? ?????? ??????!" -ForegroundColor Green
Write-Host ""
