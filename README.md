# Sahl - Advanced Cache Management System for Blazor WebAssembly

<div align="center">

![Version](https://img.shields.io/badge/version-1.0.0-blue.svg)
![Status](https://img.shields.io/badge/status-stable-green.svg)
![License](https://img.shields.io/badge/license-MIT-orange.svg)

**An intelligent version control and cache management system for enterprise Blazor WebAssembly applications**

[Overview](#-overview) • [Quick Start](#-quick-start) • [Features](#-features) • [Configuration](#-configuration) • [Support](#-support)

</div>

---

## ?? What is Sahl?

Sahl is a comprehensive **Blazor WebAssembly** version management and intelligent cache invalidation system designed to **automatically detect and deploy updates without user intervention**, ensuring your application always runs the latest version.

### Why Choose Sahl?

Struggling with Blazor WebAssembly caching issues? Experience seamless updates with:
- ? Zero downtime deployments
- ? Automatic cache invalidation
- ? Hard refresh on version change (Ctrl+F5)

### ?? Core Features

The following features are included:
- ? **Intelligent version detection** using semantic versioning
- ? **Selective cache clearing** preserving essential user data
- ? **Configurable update intervals** with customizable polling
- ? **Secure storage** for tokens and preferences
- ? **SweetAlert notifications** for user updates
- ? **Dual environment support** (Development & Production with service-worker.js & service-worker.published.js)

---

## ?? Key Features

### Version Management System
- Automatic detection of application updates
- Selective cache invalidation based on version changes
- Support for patch, minor, and major version updates

### Update Polling System
- Default check interval: 5 minutes (fully customizable)
- Smart polling prevents excessive server requests
- Tab visibility detection optimization

### Progressive Web App (PWA) Support
- Intelligent cache strategies (cache-first / network-first)
- Service Worker optimization for Development
- PWA-ready configuration for Production deployment
- Seamless offline support

### User Notification System
- Beautiful SweetAlert notifications
- Automatic update prompts
- User-friendly update dialogs

### Cross-Platform Scripting
- Windows PowerShell automation
- Batch file support for Windows environments
- Compatible with Visual Studio Code

### Security Features
- Secure token management
- Encrypted preference storage
- Protection against cache poisoning

---

## ?? Technology Stack

### Requirements
- .NET 10
- Blazor WebAssembly
- PowerShell (for automation scripts)

### Quick Setup

Ready to get started? Follow these 3 simple steps! ??

**Essential Files:**
- ?? `wwwroot/version.json`
- ?? `wwwroot/js/version-manager.js`
- ?? `wwwroot/service-worker.js` (Development)
- ?? `wwwroot/service-worker.published.js` (Production) ??
- ?? `wwwroot/index.html` (Main entry point)

### Setup Process (3 Steps):

#### Step 1?? - Update Version
```powershell
.\update-version.ps1
```
Or use the batch file: `update-version.bat`

**What it does:** Automatically updates version numbers in:
- `version.json`
- `service-worker.js`
- `service-worker.published.js`

#### Step 2?? - Build Project
```powershell
dotnet build
```

#### Step 3?? - Publish Release
```powershell
dotnet publish
```

**And that's it! Your application is ready for deployment with version management enabled!**

---

## ?? Service Worker Configuration

| File | Environment | Usage | Context |
|------|-------------|-------|---------|
| **service-worker.js** | Development | Used during development (`dotnet run`) |
| **service-worker.published.js** | Production | Used in production (`dotnet publish`) |

**Important:** Always run `update-version.ps1` before deployment to sync version numbers!

---

## ?? Version Update Types

| Type | Command | Purpose | Example |
|------|---------|---------|---------|
| **Patch** | `.\update-version.ps1` | Bug fixes | `1.0.0 ? 1.0.1` |
| **Minor** | `.\update-version.ps1 -VersionType minor` | New features | `1.0.0 ? 1.1.0` |
| **Major** | `.\update-version.ps1 -VersionType major` | Breaking changes | `1.0.0 ? 2.0.0` |
| **Force** | `.\update-version.ps1 -ForceUpdate $true` | Force update all | Manual override |

---

## ?? Project Structure

```
Project/
??? src/Presentation/Dashboard/wwwroot/
?   ??? version.json                    # Version configuration
?   ??? js/
?   ?   ??? version-manager.js          # Version management script
?   ??? service-worker.js               # Service Worker (Development)
?   ??? service-worker.published.js     # Service Worker (Production)
??? update-version.ps1                  # PowerShell update script
??? update-version.bat                  # Windows batch script
??? .vscode/
?   ??? launch.json                     # VS Code debug configuration
??? Documentation/
    ??? README_INSTALLATION.md          # Installation guide
    ??? QUICK_START.md                  # Quick start (3 steps)
    ??? CACHE_UPDATE_SYSTEM.md          # Cache management guide
    ??? API_DOCUMENTATION.md            # JavaScript API reference
    ??? EXAMPLE_VERSION_SETTINGS.html   # Example UI implementation
```

---

## ?? Documentation

### Getting Started:
- ?? **[README_INSTALLATION.md](README_INSTALLATION.md)** - Complete installation guide
- ?? **[QUICK_START.md](QUICK_START.md)** - Quick start guide (3 steps)
- ?? **[SUMMARY.md](SUMMARY.md)** - Project summary

### Technical Docs:
- ?? **[CACHE_UPDATE_SYSTEM.md](CACHE_UPDATE_SYSTEM.md)** - Cache system documentation
- ?? **[API_DOCUMENTATION.md](API_DOCUMENTATION.md)** - JavaScript API reference
- ?? **[EXAMPLE_VERSION_SETTINGS.html](EXAMPLE_VERSION_SETTINGS.html)** - Example UI

### Project History:
- ?? **[CHANGELOG.md](CHANGELOG.md)** - Version history

---

## ?? Usage Examples

### Via PowerShell:
```powershell
# Patch version (1.0.0 ? 1.0.1)
.\update-version.ps1

# Minor version (1.0.0 ? 1.1.0)
.\update-version.ps1 -VersionType minor

# Major version (1.0.0 ? 2.0.0)
.\update-version.ps1 -VersionType major

# Force update
.\update-version.ps1 -ForceUpdate $true
```

### Via JavaScript:
```javascript
// Manual update check
await VersionManager.manualCheck();

// Get current version
const version = localStorage.getItem('app_version');

// Clear all caches
await caches.keys().then(names => 
    Promise.all(names.map(name => caches.delete(name)))
);
```

### Via Blazor Component:
```razor
@inject IJSRuntime JS

<button @onclick="CheckForUpdates">Check for Updates</button>

@code {
    private async Task CheckForUpdates()
    {
        await JS.InvokeVoidAsync("VersionManager.manualCheck");
    }
}
```

---

## ?? Configuration

### Version Check Interval:
In `wwwroot/js/version-manager.js`:
```javascript
const VERSION_CHECK_INTERVAL = 180000; // 3 minutes
```

### Protected Storage Keys:
```javascript
const keysToKeep = [
    'auth_token',
    'refresh_token',
    'user_preferences',
    'selected_language',
    'my_custom_key' // Add your own
];
```

---

## ?? Testing

### Manual Testing Steps:
```powershell
# 1. Run update script
.\update-version.ps1

# 2. Check updated version in files
# 3. Open Console (F12)
# 4. Update version again
.\update-version.ps1

# 5. Wait 5 minutes for next poll
# 6. See automatic update notification!
```

### Via Console:
```javascript
// 1. Check current version
localStorage.getItem('app_version')

// 2. Force check
await VersionManager.manualCheck()

// 3. Complete clear
localStorage.clear();
sessionStorage.clear();
caches.keys().then(n => Promise.all(n.map(k => caches.delete(k))));
location.reload(true);
```

---

## ?? CI/CD Integration

### GitHub Actions:
```yaml
name: Deploy
on:
  push:
    branches: [main]

jobs:
  deploy:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v3
     
