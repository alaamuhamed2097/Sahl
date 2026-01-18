# Home Page Blocks Management Feature

## Overview

The **Home Page Blocks Management** feature allows administrators to create, configure, arrange, and manage dynamic content blocks on the e-commerce platform's homepage. Each block can display different types of content (products, categories, campaigns, personalized recommendations) with multiple layout options.

## Architecture

### Backend Components

#### 1. **Entity Layer** (`src/Core/Domains`)
- **`TbHomepageBlock.cs`** - Main entity representing a homepage block
- **`TbBlockProduct.cs`** - Products associated with a block
- **`TbBlockCategory.cs`** - Categories associated with a block

#### 2. **Enumeration** (`src/Shared/Common/Enumerations`)
- **`BlockLayout`** - Defines available layouts:
  - `Carousel` (1) - Horizontal scrolling
  - `TwoRows` (2) - Two rows layout
  - `Featured` (3) - Large product cards (1 item per card)
  - `Compact` (4) - Small product cards (4 items per card)
  - `FullWidth` (5) - Full width banner/hero
- **`HomepageBlockType`** - Block types:
  - `Manual` - Manually selected items
  - `Campaign` - Campaign-based content
  - `Dynamic` - Dynamic data sources (best sellers, new arrivals, etc.)
  - `Personalized` - User-specific recommendations

#### 3. **Service Layer** (`src/Core/BL`)
- **`IAdminBlockService`** - Contract defining all block management operations
- **`AdminBlockService`** - Implementation providing:
  - Block CRUD operations
  - Product/item management
  - Category management
  - Display order management
  - Configuration validation

#### 4. **API Layer** (`src/Presentation/Api`)
- **`AdminBlockController`** - REST endpoints for block management:
  - `POST /api/v1/admin/blocks` - Create block
  - `GET /api/v1/admin/blocks` - Get all blocks
  - `GET /api/v1/admin/blocks/{blockId}` - Get block details
  - `PUT /api/v1/admin/blocks/{blockId}` - Update block
  - `DELETE /api/v1/admin/blocks/{blockId}` - Delete block
  - `PATCH /api/v1/admin/blocks/{blockId}/display-order` - Update display order
  - `POST /api/v1/admin/blocks/{blockId}/products` - Add product to block
  - `DELETE /api/v1/admin/blocks/products/{productId}` - Remove product from block
  - `GET /api/v1/admin/blocks/{blockId}/products` - Get block products
  - `POST /api/v1/admin/blocks/{blockId}/categories` - Add category to block
  - `DELETE /api/v1/admin/blocks/categories/{categoryId}` - Remove category
  - `GET /api/v1/admin/blocks/{blockId}/categories` - Get block categories

### Frontend Components (Blazor Dashboard)

#### 1. **DTOs** (`src/Shared/Shared/DTOs/Merchandising/Homepage`)
- **`AdminBlockCreateDto`** - Block creation/update form data
- **`AdminBlockListDto`** - Block list view data
- **`AdminBlockItemDto`** - Product/item within a block
- **`AdminBlockCategoryDto`** - Category within a block

#### 2. **Services** (`src/Presentation/Dashboard/Services`)
- **`IAdminBlockService`** - Dashboard service contract
- **`AdminBlockService`** - Implementation handling API calls

#### 3. **Pages** (`src/Presentation/Dashboard/Pages/Marketing/HomeBlocks`)
- **`Index.razor / Index.razor.cs`** - Block list page with:
  - Table display of all blocks
  - Search and filtering
  - Status indicators (Active, Scheduled, Hidden)
  - Edit/Delete actions
  - Export functionality
  
- **`Details.razor / Details.razor.cs`** - Block creation/edit page with:
  - Bilingual title and subtitle inputs
  - Block type selection (Manual, Campaign, Dynamic, Personalized)
  - Layout selection
  - Visibility settings (visible flag, time-based scheduling)
  - Campaign selection (for Campaign type)
  - Dynamic source selection (for Dynamic type)
  - Personalization source selection (for Personalized type)
  - Display order configuration
  - View All link customization
  - Real-time preview

#### 4. **Navigation** 
- Added to Dashboard Navigation Menu under **Marketing > Home Page Blocks**

## User Interface Features

### List Page (`/home-blocks`)
- **Search functionality** - Search blocks by title or content
- **Status filtering** - Filter by Active, Scheduled, or Hidden status
- **Sortable columns** - Sort by title, type, layout, order, etc.
- **Action buttons** - Edit and delete blocks
- **Export capability** - Export block list to Excel or print
- **Pagination** - Navigate through large lists

### Details Page (`/home-blocks/new` or `/home-blocks/{id}`)
- **Bilingual inputs** - Support for English and Arabic titles/subtitles
- **Block type configuration** - Different UI based on selected type
- **Layout preview** - Visual representation of selected layout
- **Visibility settings** - Control when blocks appear
- **Campaign selection** - Choose campaigns for Campaign-type blocks
- **Dynamic sources** - Options for Best Sellers, New Arrivals, On Sale, Top Rated
- **Personalization** - Recommended For You, Recently Viewed, Similar Browsing
- **Display ordering** - Set block position on homepage
- **Real-time preview** - See changes as you configure

## Data Model

### TbHomepageBlock
```csharp
public class TbHomepageBlock : BaseEntity
{
    public string TitleEn { get; set; }              // English title (required)
    public string TitleAr { get; set; }              // Arabic title (required)
    public string? SubtitleEn { get; set; }          // Optional English subtitle
    public string? SubtitleAr { get; set; }          // Optional Arabic subtitle
    public HomepageBlockType Type { get; set; }      // Block type
    public DynamicBlockSource? DynamicSource { get; set; }     // For dynamic blocks
    public PersonalizationSource? PersonalizationSource { get; set; } // For personalized
    public Guid? CampaignId { get; set; }            // For campaign blocks
    public BlockLayout Layout { get; set; }          // Visual layout
    public int DisplayOrder { get; set; }            // Position on homepage
    public DateTime? VisibleFrom { get; set; }       // Scheduled start (optional)
    public DateTime? VisibleTo { get; set; }         // Scheduled end (optional)
    public bool IsVisible { get; set; }              // Visibility flag
    public bool ShowViewAllLink { get; set; }        // Show "View All" link
    public string? ViewAllLinkTitleEn { get; set; }  // View All link text (EN)
    public string? ViewAllLinkTitleAr { get; set; }  // View All link text (AR)
    
    // Collections
    public ICollection<TbBlockItem> BlockProducts { get; set; }    // Products in block
    public ICollection<TbBlockCategory> BlockCategories { get; set; } // Categories in block
}
```

## Block Types

### 1. Manual Block
- Administrators manually select specific products
- Useful for curated product selections
- No automatic updates

### 2. Campaign Block
- Displays products from a selected campaign
- Updates automatically as campaign changes
- Good for promotional content

### 3. Dynamic Block
- Displays products based on predefined criteria:
  - **Best Sellers** - Top performing products
  - **New Arrivals** - Recently added products
  - **On Sale** - Discounted products
  - **Top Rated** - Highest rated products
- Updates automatically based on data changes

### 4. Personalized Block
- Shows customized content per user:
  - **Recommended For You** - AI-based recommendations
  - **Recently Viewed** - User browsing history
  - **Similar to Your Browsing** - Related products
- Requires user context to display

## Layout Options

### Carousel
- Horizontal scrolling layout
- Ideal for showcasing multiple items
- Responsive swiper/slider implementation

### Two Rows
- Grid layout with 2 rows
- Medium-sized product cards
- Good for featured products

### Featured
- Large product cards
- 1 item per card
- High visual impact
- Best for hero/banner content

### Compact
- Small product cards
- 4+ items per card
- Maximize number of products shown
- Efficient space usage

### Full Width
- Full width banner/hero format
- Large hero image with text overlay
- Perfect for seasonal promotions

## Usage Workflow

### Creating a Block

1. Navigate to **Marketing > Home Page Blocks**
2. Click **Add Block**
3. Fill in bilingual titles
4. Select block type:
   - **Manual**: Select layout and products
   - **Campaign**: Select campaign to display
   - **Dynamic**: Choose data source (Best Sellers, etc.)
   - **Personalized**: Choose personalization source
5. Select layout (Carousel, TwoRows, Featured, Compact, FullWidth)
6. Set display order
7. Configure visibility (immediate or scheduled)
8. Add "View All" link text if needed
9. Click **Create Block**

### Editing a Block

1. Go to **Marketing > Home Page Blocks**
2. Click edit button for desired block
3. Modify settings
4. Click **Update Block**

### Managing Products in a Block

1. Open block details
2. In Products tab:
   - **Add Product**: Select from catalog
   - **Reorder**: Drag to rearrange
   - **Remove**: Delete from block
   - **Configure Display Order**: Set specific position

### Managing Categories in a Block

1. Open block details
2. In Categories tab:
   - **Add Category**: Select from catalog
   - **Reorder**: Drag to rearrange
   - **Remove**: Delete from block

### Scheduling Visibility

1. Open block creation/edit page
2. Check "Is Visible" checkbox
3. Set "Visible From" date/time (optional - immediate if blank)
4. Set "Visible To" date/time (optional - permanent if blank)
5. Save block

Block will automatically:
- Show after "Visible From" time
- Hide after "Visible To" time
- Display "Hidden" status if visibility window hasn't started
- Display "Scheduled" status if within scheduled time
- Display "Active" status if currently visible

## Status Indicators

### Active (Green)
- Block is visible and currently within visibility window
- Displaying on homepage to users

### Scheduled (Yellow)
- Block has visibility scheduling configured
- Will become active at scheduled time
- Currently not visible to users

### Hidden (Red)
- Block is not visible
- Either visibility is disabled or visibility window has passed
- Not displaying on homepage

## Features Implemented

? **Block Management**
- Create blocks with bilingual support
- Edit block configuration
- Delete blocks (soft delete)
- View all blocks with search and filter

? **Content Management**
- Add/remove products from blocks
- Add/remove categories from blocks
- Reorder items within blocks
- Manage display order of blocks

? **Configuration Options**
- Multiple block types (Manual, Campaign, Dynamic, Personalized)
- Multiple layout options (Carousel, TwoRows, Featured, Compact, FullWidth)
- Bilingual titles and subtitles
- Optional subtitles
- Customizable "View All" links

? **Visibility Control**
- Toggle block visibility
- Schedule visibility (from/to dates)
- Auto-hide based on time windows
- Status indicators (Active, Scheduled, Hidden)

? **UI/UX**
- Responsive admin dashboard
- List view with search, filter, sort, export
- Detail view with form validation
- Real-time preview panel
- Notification feedback for actions

## Service Registration

The `AdminBlockService` is registered in `DomainServiceExtensions.cs`:

```csharp
services.AddScoped<IAdminBlockService, AdminBlockService>();
```

## Authorization

All admin block endpoints require:
- **Role**: `Admin`
- **Authentication**: JWT Bearer token with NameIdentifier claim

## Error Handling

All endpoints include:
- Validation error responses (400 Bad Request)
- Not found responses (404 Not Found)
- Server error handling (500 Internal Server Error)
- Detailed error messages for debugging

## Next Steps / Future Enhancements

1. **Drag & Drop Reordering**
   - Implement drag-and-drop for blocks and items
   - Save new order via API

2. **Content Preview**
   - Live preview of how blocks will appear on homepage
   - Mobile/tablet responsive preview

3. **Performance Optimization**
   - Implement caching for frequently accessed blocks
   - Pagination for large product lists

4. **Analytics Integration**
   - Track block engagement and clicks
   - A/B testing for different layouts

5. **Advanced Personalization**
   - Machine learning-based recommendations
   - User segment targeting

6. **Bulk Operations**
   - Bulk edit multiple blocks
   - Bulk import/export blocks

7. **Template System**
   - Create block templates
   - Quick block creation from templates

## Testing

To test the feature:

1. **Create a block**:
   - POST `/api/v1/admin/blocks` with block data
   - Verify block appears in list

2. **Update a block**:
   - PUT `/api/v1/admin/blocks/{blockId}` with modified data
   - Verify changes are saved

3. **Add products**:
   - POST `/api/v1/admin/blocks/{blockId}/products`
   - Verify products appear in block

4. **Schedule visibility**:
   - Create block with VisibleFrom in future
   - Verify status shows "Scheduled"

5. **Test layouts**:
   - Create blocks with different layouts
   - Verify frontend renders correctly

## Dependencies

- Blazor Components for UI
- Bootstrap 5 for styling
- ASP.NET Core API for backend
- Entity Framework Core for data access
- AutoMapper for DTO mapping

## Notes

- All block operations are soft deletes (IsDeleted flag)
- Bilingual support is built-in (English/Arabic)
- Display order affects homepage rendering order
- Visibility scheduling uses UTC time
- All timestamps stored in UTC for consistency
