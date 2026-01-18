using Dashboard.Contracts.Merchandising;
using Dashboard.Contracts.Notification;
using Microsoft.AspNetCore.Components;
using Shared.DTOs.Merchandising.Homepage;
using Shared.GeneralModels;

namespace Dashboard.Pages.Merchandising.HomeBlocks
{
    public partial class Index : BaseListPage<AdminBlockListDto>
    {
        [Inject] protected IAdminBlockService AdminBlockService { get; set; } = null!;
        [Inject] protected INotificationService NotificationService { get; set; } = null!;
        [Inject] protected NavigationManager NavigationManager { get; set; } = null!;

        protected override string EntityName { get; } = "Home Page Blocks";
        protected override string AddRoute { get; } = "/home-blocks/new";
        protected override string EditRouteTemplate { get; } = "/home-blocks/{id}";
        protected override string SearchEndpoint { get; } = "api/v1/admin/blocks";

        // Initialize items collection to prevent null reference
        protected new IEnumerable<AdminBlockListDto> items = new List<AdminBlockListDto>();

        // Store status filter separately
        protected string currentStatusFilter = string.Empty;

        protected override Dictionary<string, Func<AdminBlockListDto, object>> ExportColumns { get; } =
            new()
            {
                ["Title (EN)"] = x => x.TitleEn,
                ["Title (AR)"] = x => x.TitleAr,
                ["Type"] = x => x.Type,
                ["Layout"] = x => x.Layout,
                ["Display Order"] = x => x.DisplayOrder,
                ["Visible"] = x => x.IsVisible ? "Yes" : "No",
                ["Status"] = x => x.StatusBadge,
                ["Created"] = x => x.CreatedDateUtc.ToString("yyyy-MM-dd")
            };

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            await Search();
        }

        protected override async Task<ResponseModel<IEnumerable<AdminBlockListDto>>> GetAllItemsAsync()
        {
            try
            {
                var response = await AdminBlockService.GetAllBlocksAsync();
                return new ResponseModel<IEnumerable<AdminBlockListDto>>
                {
                    Success = response.Success,
                    Data = response.Data,
                    Message = response.Message
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel<IEnumerable<AdminBlockListDto>>
                {
                    Success = false,
                    Data = new List<AdminBlockListDto>(),
                    Message = ex.Message
                };
            }
        }

        protected override async Task<ResponseModel<bool>> DeleteItemAsync(Guid id)
        {
            try
            {
                var response = await AdminBlockService.DeleteBlockAsync(id);
                return new ResponseModel<bool>
                {
                    Success = response.Success,
                    Message = response.Message
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel<bool>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        protected override async Task<string> GetItemId(AdminBlockListDto item)
        {
            return item?.Id != Guid.Empty ? item.Id.ToString() : string.Empty;
        }

        protected virtual async Task OnStatusFilterChanged(string status)
        {
            currentStatusFilter = status;
            searchModel.PageNumber = 1;
            await Search();
        }

        /// <summary>
        /// Override Search to use custom filtering instead of SearchService
        /// </summary>
        protected override async Task Search()
        {
            try
            {
                StateHasChanged();

                await OnBeforeSearchAsync();

                // Get all blocks from the service
                var response = await AdminBlockService.GetAllBlocksAsync();

                if (response.Success && response.Data != null)
                {
                    var allBlocks = response.Data.AsEnumerable();

                    // Apply search term filter (client-side)
                    if (!string.IsNullOrWhiteSpace(searchModel.SearchTerm))
                    {
                        allBlocks = allBlocks.Where(b =>
                            b.TitleEn.Contains(searchModel.SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                            b.TitleAr.Contains(searchModel.SearchTerm, StringComparison.OrdinalIgnoreCase));
                    }

                    // Apply status filter (client-side)
                    if (!string.IsNullOrEmpty(currentStatusFilter))
                    {
                        allBlocks = allBlocks.Where(b => b.StatusBadge.Equals(currentStatusFilter, StringComparison.OrdinalIgnoreCase));
                    }

                    // Apply sorting
                    if (!string.IsNullOrEmpty(searchModel.SortBy))
                    {
                        allBlocks = searchModel.SortDirection == "desc"
                            ? allBlocks.OrderByDescending(GetPropertyValue)
                            : allBlocks.OrderBy(GetPropertyValue);
                    }
                    else
                    {
                        // Default sort by DisplayOrder
                        allBlocks = allBlocks.OrderBy(b => b.DisplayOrder);
                    }

                    // Get total count before pagination
                    var filteredList = allBlocks.ToList();
                    totalRecords = filteredList.Count;
                    totalPages = (int)Math.Ceiling((double)totalRecords / searchModel.PageSize);

                    // Apply pagination (client-side)
                    var pageNumber = Math.Max(1, searchModel.PageNumber);
                    var skip = (pageNumber - 1) * searchModel.PageSize;
                    items = filteredList.Skip(skip).Take(searchModel.PageSize).ToList();

                    currentPage = pageNumber;

                    await OnAfterSearchAsync();
                }
                else
                {
                    items = new List<AdminBlockListDto>();
                    totalRecords = 0;
                    totalPages = 0;
                }
            }
            catch (Exception ex)
            {
                await NotificationService.ShowErrorAsync($"Error loading blocks: {ex.Message}");
                items = new List<AdminBlockListDto>();
                totalRecords = 0;
                totalPages = 0;
            }
            finally
            {
                StateHasChanged();
            }
        }

        /// <summary>
        /// Clear search and filters
        /// </summary>
        protected async Task ClearSearch()
        {
            searchModel.SearchTerm = string.Empty;
            currentStatusFilter = string.Empty;
            searchModel.PageNumber = 1;
            await Search();
        }

        /// <summary>
        /// Helper to get property value for sorting
        /// </summary>
        private object GetPropertyValue(AdminBlockListDto block)
        {
            return searchModel.SortBy?.ToLower() switch
            {
                "titlefull" => block.TitleEn,
                "type" => block.Type,
                "layout" => block.Layout,
                "displayorder" => block.DisplayOrder,
                "created" => block.CreatedDateUtc,
                _ => block.DisplayOrder
            };
        }

        // ====================================
        // Reorder Methods
        // ====================================

        /// <summary>
        /// Move block up in order
        /// </summary>
        protected async Task MoveBlockUp(AdminBlockListDto block)
        {
            var blocksList = items.OrderBy(b => b.DisplayOrder).ToList();
            var currentIndex = blocksList.FindIndex(b => b.Id == block.Id);

            if (currentIndex <= 0)
            {
                await NotificationService.ShowErrorAsync("Block is already at the top");
                return;
            }

            var targetBlock = blocksList[currentIndex - 1];
            await ReorderBlocks(block, targetBlock);
        }

        /// <summary>
        /// Move block down in order
        /// </summary>
        protected async Task MoveBlockDown(AdminBlockListDto block)
        {
            var blocksList = items.OrderBy(b => b.DisplayOrder).ToList();
            var currentIndex = blocksList.FindIndex(b => b.Id == block.Id);

            if (currentIndex >= blocksList.Count - 1)
            {
                await NotificationService.ShowErrorAsync("Block is already at the bottom");
                return;
            }

            var targetBlock = blocksList[currentIndex + 1];
            await ReorderBlocks(block, targetBlock);
        }

        /// <summary>
        /// Core reordering logic
        /// </summary>
        private async Task ReorderBlocks(AdminBlockListDto draggedBlock, AdminBlockListDto targetBlock)
        {
            if (draggedBlock.Id == targetBlock.Id)
            {
                return;
            }

            try
            {
                // Get all items in current page
                var blocksList = items.OrderBy(b => b.DisplayOrder).ToList();

                var draggedIndex = blocksList.FindIndex(b => b.Id == draggedBlock.Id);
                var targetIndex = blocksList.FindIndex(b => b.Id == targetBlock.Id);

                if (draggedIndex == -1 || targetIndex == -1)
                {
                    await NotificationService.ShowErrorAsync("Invalid reorder operation");
                    return;
                }

                // Remove dragged block
                var movedBlock = blocksList[draggedIndex];
                blocksList.RemoveAt(draggedIndex);

                // Insert at new position
                blocksList.Insert(targetIndex, movedBlock);

                // Update display orders for ALL blocks (not just changed ones)
                var updateTasks = new List<Task>();
                for (int i = 0; i < blocksList.Count; i++)
                {
                    var block = blocksList[i];
                    var newOrder = ((currentPage - 1) * searchModel.PageSize) + i;

                    // Always update to ensure correct order
                    updateTasks.Add(AdminBlockService.UpdateDisplayOrderAsync(block.Id, newOrder));
                }

                // Execute all updates
                await Task.WhenAll(updateTasks);

                // Refresh the list to show new order
                await Search();

                await NotificationService.ShowSuccessAsync("Block order updated successfully!");
            }
            catch (Exception ex)
            {
                await NotificationService.ShowErrorAsync($"Error reordering blocks: {ex.Message}");
            }
            finally
            {
                StateHasChanged();
            }
        }

        // ====================================
        // Navigation & Actions
        // ====================================

        protected void NavigateToEdit(Guid blockId)
        {
            NavigationManager.NavigateTo($"/home-blocks/{blockId}");
        }

        protected async Task ConfirmDelete(Guid blockId)
        {
            await Delete(blockId);
        }

        // ====================================
        // Pagination
        // ====================================

        protected async Task PreviousPage()
        {
            if (currentPage > 1)
            {
                searchModel.PageNumber = currentPage - 1;
                await Search();
            }
        }

        protected async Task NextPage()
        {
            if (currentPage < totalPages)
            {
                searchModel.PageNumber = currentPage + 1;
                await Search();
            }
        }

        protected async Task GoToPage(int page)
        {
            if (page >= 1 && page <= totalPages && page != currentPage)
            {
                searchModel.PageNumber = page;
                await Search();
            }
        }

        // ====================================
        // Helper Methods
        // ====================================

        protected string GetStatusClass(string status)
        {
            return status switch
            {
                "Active" => "badge-active",
                "Hidden" => "badge-hidden",
                "Scheduled" => "badge-scheduled",
                _ => "badge-active"
            };
        }

        protected string GetTypeIcon(string type)
        {
            return type switch
            {
                "Manual" => "🎯",
                "Campaign" => "🏷️",
                "Dynamic" => "⚡",
                "Personalized" => "👤",
                _ => "📦"
            };
        }

        protected string GetLayoutIcon(string layout)
        {
            return layout switch
            {
                "Carousel" => "🎠",
                "Grid" => "📊",
                "Featured" => "⭐",
                "Compact" => "📦",
                "FullWidth" => "📺",
                "TwoRows" => "📋",
                _ => "🔲"
            };
        }

        // ====================================
        // Filter & Pagination
        // ====================================

        protected async Task OnPageSizeChanged(ChangeEventArgs e)
        {
            if (int.TryParse(e.Value?.ToString(), out int pageSize))
            {
                searchModel.PageSize = pageSize;
                searchModel.PageNumber = 1;
                await Search();
            }
        }
    }
}