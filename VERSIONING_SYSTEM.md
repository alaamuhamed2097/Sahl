# Version Management & Cache Control System

## Overview
This system provides automatic version checking and cache management for the Blazor WebAssembly application. When you deploy a new version, users will automatically receive updates without manually clearing their browser cache.

## How It Works

### 1. Version Detection
- The `version.json` file contains the current application version
- The system checks for updates every 5 minutes
- Checks also occur when users return to the tab/window
- Service worker fetches the version dynamically on activation

### 2. Automatic Cache Clearing
When a version change is detected:
1. All browser caches are cleared automatically
2. Service workers are unregistered and re-registered
3. Non-essential localStorage is cleared (authentication is preserved)
4. The page automatically reloads with the new version

### 3. Files Involved

#### `version.json`
```json
{
    "buildDate": "2025-11-13T11:46:49Z",
    "version": "1.0.9",
    "forceUpdate": true
}
```
- **version**: Application version number (increment for each release)
- **forceUpdate**: Set to `true` to force immediate update for all users
- **buildDate**: Timestamp of the build (for reference)

#### `service-worker.js`
- Handles offline caching with dynamic version management
- Automatically fetches latest version on activation
- Clears old caches when version changes
- Sends update notifications to the version manager

#### `service-worker.published.js`
- Published version of service worker for production builds
- Same functionality as service-worker.js but optimized for production

#### `version-manager.js`
- Client-side version checking and update orchestration
- Registers and manages service worker lifecycle
- Handles automatic cache clearing and page reloads
- Preserves user authentication during updates

#### `cache-buster.js`
- Manual cache clearing utility for debugging
- Provides console commands for cache management

## Deployment Workflow

### When Deploying a New Version:

1. **Update version.json**:
   ```json
   {
       "buildDate": "2025-01-15T10:30:00Z",
       "version": "1.0.10",
       "forceUpdate": true
   }
   ```

2. **Build and deploy** your application as usual

3. **Users receive updates automatically**:
   - Within 5 minutes (periodic check)
   - Immediately when they return to the tab
   - Service worker detects the change and triggers update

### Version Numbering Strategy:
- **Patch**: 1.0.X - Bug fixes, minor updates
- **Minor**: 1.X.0 - New features, enhancements
- **Major**: X.0.0 - Breaking changes, major redesigns

## Force Update

Set `"forceUpdate": true` in version.json when:
- Critical security fixes
- Breaking changes that require immediate deployment
- Database schema changes
- API contract changes

After all users have updated, you can set it back to `false`.

## User Experience

### Automatic Update Flow:
1. User is using the application
2. System detects new version (background check)
3. Caches are cleared automatically
4. Page reloads seamlessly
5. User continues with new version (login preserved)

### No Manual Intervention Required:
- ? No need to press Ctrl+F5
- ? No need to clear browser cache manually
- ? No need to close/reopen browser
- ? Authentication is preserved

## Debugging & Troubleshooting

### Console Commands (for developers/admins):

#### Check Current Cache Status:
```javascript
CacheBuster.checkCacheStatus()
```
Shows:
- Active caches and their sizes
- Service worker status
- Current version vs server version
- Storage usage

#### Force Cache Clear and Reload:
```javascript
CacheBuster.clearAllAndReload()
```
Clears everything and reloads (authentication preserved)

#### Manual Version Check:
```javascript
VersionManager.manualCheck()
```
Forces an immediate version check

#### Quick Reset (with confirmation):
```javascript
CacheBuster.quickReset()
```
User-friendly reset with confirmation dialog

### Common Issues:

#### Issue: User still sees old version after deployment
**Solution**:
1. Check that version.json was deployed with new version number
2. Verify forceUpdate is set to true
3. User should wait 5 minutes or refresh the page
4. If still not working, user can run: `CacheBuster.clearAllAndReload()`

#### Issue: Service worker not updating
**Solution**:
1. Service worker file may be cached by CDN/proxy
2. Add cache-busting headers to service worker files in web server config
3. Use `updateViaCache: 'none'` option (already implemented)

#### Issue: Users logged out after update
**Solution**:
- Check that authentication tokens are in the keysToKeep list in version-manager.js
- Verify sessionStorage is NOT being cleared (it contains Blazor state)

## Configuration

### Adjust Update Check Interval:
In `version-manager.js`, modify:
```javascript
const VERSION_CHECK_INTERVAL = 300000; // 5 minutes (in milliseconds)
```

### Preserve Additional localStorage Keys:
In `version-manager.js`, add to keysToKeep:
```javascript
const keysToKeep = [
    'auth_token',
    'your_custom_key',
    // ... add more keys here
];
```

### Disable Automatic Updates:
Comment out the initialization in `index.html`:
```html
<!-- <script src="js/version-manager.js"></script> -->
```

## Best Practices

1. **Always test in staging first**: Deploy to staging environment and verify update process works correctly

2. **Use semantic versioning**: Follow X.Y.Z pattern consistently

3. **Update buildDate**: Keep build date accurate for tracking

4. **Monitor update success**: Check server logs to ensure users are receiving updates

5. **Coordinate with users**: For major updates, notify users in advance

6. **Keep forceUpdate false by default**: Only enable for critical updates to avoid disrupting users

## Technical Details

### Cache Strategy:
- **Network-first** for API calls
- **Cache-first** for static assets (with background update)
- **Never cache** _framework files, version.json, or service workers

### Update Detection:
- Compares server version with locally stored version
- Uses cache-busting query parameters (?t=timestamp)
- Forces no-cache headers on version.json requests

### Preserved Data During Update:
- Authentication tokens (auth_token, refresh_token, etc.)
- User preferences
- Language selection
- Theme settings
- Session storage (Blazor state)

### Cleared Data During Update:
- All browser caches (Cache API)
- Non-essential localStorage items
- Old service worker registrations

## Testing the System

### Manual Test Procedure:

1. **Deploy current version** (e.g., 1.0.9)
2. **Load application** and verify it works
3. **Check console** for version logs:
   ```
   ?? Current Version: 1.0.9
   ?? Server Version: 1.0.9
   ? App is up to date
   ```

4. **Update version.json** to 1.0.10 (don't reload page yet)
5. **Wait 5 minutes** OR run `VersionManager.manualCheck()`
6. **Observe automatic reload** with new version
7. **Verify authentication preserved** (user still logged in)

## Monitoring

### Logs to Monitor:
- Browser console logs (prefixed with ??, ??, ?, ??, ?)
- Service worker activation logs
- Version check timestamps
- Cache clearing events

### Success Indicators:
- `? Service Worker: Activated and claimed clients`
- `?? Version updated to: X.X.X`
- `? All caches cleared`
- `?? Reloading application...`

## Support

For issues or questions about the versioning system:
1. Check browser console for error messages
2. Run `CacheBuster.checkCacheStatus()` to diagnose
3. Review logs for version check failures
4. Verify version.json is accessible at /version.json

---

**Last Updated**: 2025-01-15
**System Version**: 2.0.0
**Compatibility**: Modern browsers with Service Worker support (Chrome 45+, Firefox 44+, Safari 11.1+, Edge 17+)
