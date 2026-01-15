# Quick Start Guide: Home Page Blocks Management

## Access the Feature

### Dashboard URL
```
https://[your-domain]/home-blocks
```

### Navigation Path
**Dashboard Menu ? Marketing ? Home Page Blocks**

## Core Operations

### 1. Create a New Block

```
1. Click "Add Block" button
2. Enter Title (English) - Required
3. Enter Title (Arabic) - Required
4. Optionally add subtitles
5. Select Block Type:
   - Manual: Pick specific products
   - Campaign: Link to a campaign
   - Dynamic: Use data source (Best Sellers, New Arrivals, etc.)
   - Personalized: User recommendations
6. Select Layout:
   - Carousel: Horizontal scrolling
   - TwoRows: 2-row grid
   - Featured: Large cards
   - Compact: Small cards, many items
   - FullWidth: Full-width banner
7. Set Display Order (position on homepage)
8. Configure Visibility:
   - Check "Is Visible" to enable
   - Set "Visible From" for scheduled start
   - Set "Visible To" for scheduled end
9. Optionally configure "View All" link text
10. Click "Create Block"
```

### 2. Edit an Existing Block

```
1. Go to /home-blocks
2. Find the block in the list
3. Click the edit icon (pencil)
4. Modify any settings
5. Click "Update Block"
```

### 3. Delete a Block

```
1. Go to /home-blocks
2. Find the block in the list
3. Click the delete icon (trash)
4. Confirm deletion
```

### 4. Search for Blocks

```
1. Go to /home-blocks
2. Use the search box to find by title
3. Results update automatically
```

### 5. Filter by Status

```
1. Go to /home-blocks
2. Use Status dropdown:
   - All Status: Show all blocks
   - Active: Currently visible blocks
   - Scheduled: Future/past scheduled blocks
   - Hidden: Disabled blocks
```

## Block Type Examples

### Manual Block
**Use Case**: Curated product selection, staff picks
```
1. Create block with type "Manual"
2. Select "Featured" layout for showcase
3. Product selection tab - add specific products
4. Set display order to control homepage position
```

### Campaign Block
**Use Case**: Promotional campaign display
```
1. Create block with type "Campaign"
2. Select campaign from dropdown
3. Products auto-populate from campaign
4. Use "Carousel" layout for multiple items
5. Set order based on campaign importance
```

### Dynamic Block
**Use Case**: Always-current content (Best Sellers, etc.)
```
1. Create block with type "Dynamic"
2. Select data source:
   - Best Sellers: Top-performing products
   - New Arrivals: Recently added products
   - On Sale: Discounted products
   - Top Rated: Highest-rated products
3. Content updates automatically
4. Use "Compact" layout to show many items
```

### Personalized Block
**Use Case**: User-specific recommendations
```
1. Create block with type "Personalized"
2. Select source:
   - Recommended For You: AI-based suggestions
   - Recently Viewed: User's browsing history
   - Similar to Your Browsing: Related items
3. Each user sees different content
4. Great for engagement and conversion
```

## Scheduling Blocks

### Schedule Start Time
```
1. Edit block
2. Set "Visible From" to future date/time
3. Block shows "Scheduled" status until that time
4. Automatically becomes "Active" at scheduled time
```

### Schedule End Time
```
1. Edit block
2. Set "Visible To" to end date/time
3. Block automatically hides after that time
4. Useful for limited-time promotions
```

### Remove Scheduling
```
1. Edit block
2. Clear "Visible From" and "Visible To" fields
3. Block becomes permanent (until manually hidden)
```

## Status Meanings

| Status | Meaning | Action |
|--------|---------|--------|
| **Active** (??) | Currently visible to users | Block is displaying on homepage |
| **Scheduled** (??) | Will be visible later | Waiting for start time or already passed end time |
| **Hidden** (??) | Not visible to users | Either visibility is off or outside scheduled window |

## Tips & Best Practices

### Layout Selection
- **Carousel**: 3-5 products, horizontal scrolling
- **TwoRows**: 6-8 products, mobile-friendly
- **Featured**: 1-3 products, high-impact showcase
- **Compact**: 12+ products, comprehensive view
- **FullWidth**: 1-2 hero images, seasonal themes

### Display Order
- Lower numbers appear higher on page
- Order 0 = top, Order 5 = below, etc.
- Campaigns often use Order 0-2 for prominence
- New Arrivals typically Order 2-3
- Best Sellers Order 3-4

### Bilingual Content
- Always fill both English and Arabic titles
- Keep titles similar length for UI consistency
- Test RTL (Right-to-Left) display for Arabic

### "View All" Links
- Enable for product collections
- Disable for hero banners
- Use consistent link text across blocks
- Examples: "Shop Now", "See More", "Browse All"

### Visibility Scheduling
- Use for seasonal content
- Schedule promotions 24 hours before start
- Schedule end 1 hour after actual end time
- Test scheduling with small blocks first

## Common Tasks

### Rotate Featured Products
```
Option 1: Manual Block
- Create 2-3 "Featured" blocks
- Schedule different products for different months
- Rotate monthly automatically

Option 2: Dynamic Block
- Use "Best Sellers" layout
- Content updates as sales change
- No manual updates needed
```

### Show Campaign Across Homepage
```
1. Create Campaign block with Carousel layout
2. Set Order 0 for top visibility
3. Schedule from campaign start to end
4. Products auto-sync with campaign
```

### Personalize User Experience
```
1. Create Personalized block "Recommended For You"
2. Use "Compact" layout to show more items
3. Place after main content sections
4. Shows different products to each user
```

### Create Email Campaign Preview
```
1. Create Featured block
2. Use FullWidth layout
3. Schedule 1-2 days before email send
4. Links directly to featured products
```

## Troubleshooting

| Issue | Solution |
|-------|----------|
| Block doesn't show on homepage | Check "Is Visible" checkbox and visibility window |
| Campaign block has no products | Ensure campaign has products assigned |
| Dynamic block shows old products | Check if data source threshold (Best Sellers count, etc.) changed |
| Arabic text not displaying | Ensure RTL CSS is loaded, check character encoding |
| Scheduled time not working | Verify server time is correct, use UTC time |
| Can't select campaign | Ensure campaign exists and is active |

## API Endpoints Reference

For developers integrating with API directly:

```
Create Block
POST /api/v1/admin/blocks
Body: AdminBlockCreateDto

Get All Blocks
GET /api/v1/admin/blocks

Get Single Block
GET /api/v1/admin/blocks/{blockId}

Update Block
PUT /api/v1/admin/blocks/{blockId}
Body: AdminBlockCreateDto

Delete Block
DELETE /api/v1/admin/blocks/{blockId}

Add Product to Block
POST /api/v1/admin/blocks/{blockId}/products
Body: { itemId: guid, displayOrder: int }

Remove Product from Block
DELETE /api/v1/admin/blocks/products/{productId}

Get Block Products
GET /api/v1/admin/blocks/{blockId}/products

Add Category to Block
POST /api/v1/admin/blocks/{blockId}/categories
Body: { categoryId: guid, displayOrder: int }

Remove Category from Block
DELETE /api/v1/admin/blocks/categories/{categoryId}

Get Block Categories
GET /api/v1/admin/blocks/{blockId}/categories

Update Display Order
PATCH /api/v1/admin/blocks/{blockId}/display-order
Body: { newOrder: int }
```

## Support & Resources

- **Feature Documentation**: See `HOME_BLOCKS_FEATURE.md`
- **Implementation Details**: See `HOMEBLOCKS_IMPLEMENTATION_SUMMARY.md`
- **API Documentation**: Check `AdminBlockController` comments
- **UI Components**: Located in `/src/Presentation/Dashboard/Pages/Marketing/HomeBlocks/`

---

**Last Updated**: 2024
**Feature Status**: ? Production Ready
**Build Status**: ? Successful Compilation
