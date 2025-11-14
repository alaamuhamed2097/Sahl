# ?? Production Deployment Fix - JavaScript Files Not Loading

## ? Problem
In production, JavaScript files return HTML instead of their actual content, causing:
```
Uncaught SyntaxError: Unexpected token '<'
```

This happens for:
- `wwwroot/assets/js/pages/product-wizard.js`
- `fileDownloadHelper.js`
- `excelExportHelper.js`

## ? Root Cause
1. **Static files not copied to publish output**
2. **IIS/Web Server rewrites all requests to index.html** (Blazor SPA fallback)
3. **Missing `web.config` rules** to exclude static files from rewriting

## ?? Solution Applied

### 1. ? Updated `Dashboard.csproj`
Added explicit Content items to ensure JavaScript files are published.

### 2. ? Created `wwwroot/web.config`
Added IIS rewrite rules to prevent JavaScript files from being rewritten.

## ?? Deployment Checklist

### Before Publishing:
- [ ] Clean solution: `dotnet clean`
- [ ] Rebuild: `dotnet build -c Release`
- [ ] Publish: `dotnet publish -c Release`

### After Publishing:
1. ? **Verify files exist** in `bin/Release/net10.0/publish/wwwroot/`
2. ? **Check file sizes** (JavaScript code, not HTML)
3. ? **Test locally** before deploying
4. ? **Verify web.config** is in the root folder
5. ? **Test URLs directly** in browser

### On Production Server (IIS):
1. ? Ensure `web.config` is in the root folder
2. ? Verify "URL Rewrite" module is installed
3. ? Check "Static Content" is enabled
4. ? Test JavaScript URLs directly
5. ? Clear browser cache (Ctrl+Shift+R)

## ?? Debugging

### Check if files are published:
```bash
cd bin/Release/net10.0/publish/wwwroot
cat assets/js/pages/product-wizard.js
```

### Test with browser DevTools:
1. Open F12 ? Network tab
2. Reload page
3. Check JavaScript file responses (should be code, not HTML)

### Test MIME type:
```bash
curl -I https://yourdomain.com/assets/js/pages/product-wizard.js
```
Should return: `Content-Type: application/javascript`

## ?? Common Issues

### Issue 1: "URL Rewrite module not installed"
**Solution:** Install from https://www.iis.net/downloads/microsoft/url-rewrite

### Issue 2: Files still return HTML
**Solution:** 
1. Check `web.config` is in root folder
2. Verify rewrite rules order
3. Clear browser cache
4. Restart IIS: `iisreset`

### Issue 3: 404 errors
**Solution:**
1. Verify files exist in publish folder
2. Check file permissions
3. Verify publish included wwwroot files

## ? Expected Results After Fix

1. ? JavaScript files load correctly
2. ? Product wizard initializes
3. ? Excel import/export works
4. ? No console errors

---

**Status:** ? Production Ready
