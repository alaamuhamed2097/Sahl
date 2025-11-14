# ?? Quick Fix Reference Card

## ?? Problem
Production errors: `Unexpected token '<'` in JavaScript files

## ? Quick Fix (3 Steps)

### 1?? Verify Fixes Applied
```powershell
# Check these files have been modified:
git status
# Should show:
# - ResourceLoaderService.cs (modified)
# - Dashboard.csproj (modified)
# - version-manager.js (modified)
```

### 2?? Rebuild & Publish
```powershell
cd src/Presentation/Dashboard
dotnet clean
dotnet build -c Release
dotnet publish -c Release -o ./publish
```

### 3?? Deploy & Test
```powershell
# Copy publish/wwwroot/* to production
# Then test in browser console (F12):
# Should see: "? Version Manager: Initializing..."
```

---

## ?? Quick Tests

### Test 1: Files Exist?
```powershell
ls publish/wwwroot/version.json
ls publish/wwwroot/js/version-manager.js
ls publish/wwwroot/assets/js/pages/product-wizard.js
```
? All three should exist

### Test 2: URLs Work?
Open in browser:
- `https://yourdomain.com/version.json`
- `https://yourdomain.com/js/version-manager.js`

? Should see code/JSON, not HTML

### Test 3: Console Clean?
Press F12, reload page:

? Should see:
```
?? Version Manager: Initializing...
? Version stored: 1.0.9
```

? Should NOT see:
```
Unexpected token '<'
Unexpected token '', "d
```

---

## ??? Emergency Fixes

### If version-manager.js fails:
Comment out in `index.html`:
```html
<!-- <script src="js/version-manager.js"></script> -->
```

### If product-wizard.js fails:
Check in Details.razor.cs:
```csharp
await ResourceLoaderService.LoadScriptsSequential(
    "assets/plugins/smart-wizard/js/jquery.smartWizard.min.js",
    "assets/js/pages/product-wizard.js"
);
```

### Clear Everything:
```javascript
// In browser console:
localStorage.clear();
caches.keys().then(k => Promise.all(k.map(c => caches.delete(c))));
location.reload(true);
```

---

## ?? Help Needed?

1. **Read:** `COMPLETE_FIX_SUMMARY.md`
2. **Version Manager Issues:** `VERSION_MANAGER_FIX.md`
3. **General Issues:** `PRODUCTION_FIX_SUMMARY.md`

---

## ? Success Indicators

- [ ] No console errors
- [ ] Product wizard loads
- [ ] Version manager initializes
- [ ] All .js files return code
- [ ] All .json files return JSON
- [ ] Network tab shows 200 OK

---

**Status:** ? Ready to Deploy  
**Build:** ? Successful  
**Priority:** ?? High

