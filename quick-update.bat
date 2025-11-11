@echo off
REM Quick Update Launcher
REM Double-click this file for quick version updates

echo.
echo ================================================
echo   Quick Version Update
echo ================================================
echo.
echo Select update type:
echo.
echo 1. Patch (1.0.0 -^> 1.0.1) [Default]
echo 2. Minor (1.0.0 -^> 1.1.0)
echo 3. Major (1.0.0 -^> 2.0.0)
echo 4. Force Update (patch + force)
echo 5. Exit
echo.

set /p choice="Enter your choice (1-5): "

if "%choice%"=="1" (
    echo.
    echo Running: Patch Update
    powershell -ExecutionPolicy Bypass -File "%~dp0quick-update.ps1"
) else if "%choice%"=="2" (
    echo.
    echo Running: Minor Update
    powershell -ExecutionPolicy Bypass -File "%~dp0quick-update.ps1" -Type minor
) else if "%choice%"=="3" (
    echo.
    echo Running: Major Update
    powershell -ExecutionPolicy Bypass -File "%~dp0quick-update.ps1" -Type major
) else if "%choice%"=="4" (
    echo.
    echo Running: Force Update
    powershell -ExecutionPolicy Bypass -File "%~dp0quick-update.ps1" -Force
) else if "%choice%"=="5" (
    echo.
    echo Exiting...
    exit /b 0
) else (
    echo.
    echo Invalid choice! Running default (Patch)...
    powershell -ExecutionPolicy Bypass -File "%~dp0quick-update.ps1"
)

echo.
echo ================================================
echo.
pause
