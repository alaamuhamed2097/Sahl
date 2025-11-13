using Dashboard.Constants;
using Dashboard.Contracts.ECommerce.Category;
using Dashboard.Contracts.General;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Resources;
using Resources.Enumerations;
using Shared.DTOs.ECommerce;
using Shared.DTOs.ECommerce.Category;
using Shared.GeneralModels.SearchCriteriaModels;
namespace Dashboard.Pages.Catalog.Categories
{
    public partial class Index
    {
        private bool isMoving = false;
        protected bool IsRTL { get; set; }
        protected IEnumerable<CategoryDto> ListCategories = Enumerable.Empty<CategoryDto>();
        protected IEnumerable<CategoryDto> TreeCategories = Enumerable.Empty<CategoryDto>();
        protected IEnumerable<CategoryDto> parents = Enumerable.Empty<CategoryDto>();
        protected CategoryDto currentItem = new() { CategoryAttributes = new List<CategoryAttributeDto>() };
        protected BaseSearchCriteriaModel searchModel = new() { PageSize = 10 };
        private int CurrentPage = 1;
        private int TotalRecords;
        private int TotalPages;
        // Tree view properties
        protected bool isTreeView = true;
        protected List<CategoryTreeNode> treeNodes = new();
        protected HashSet<Guid> expandedNodes = new();
        [Inject] protected ICategoryService CategoryService { get; set; } = null!;
        [Inject] protected ISearchService<CategoryDto> SearchService { get; set; } = null!;
        [Inject] protected IResourceLoaderService ResourceLoaderService { get; set; } = null!;
        [Inject] protected IJSRuntime JSRuntime { get; set; } = null!;
        [Inject] protected NavigationManager Navigation { get; set; } = null!;
        protected override async Task OnInitializedAsync()
        {
            var lang = await JSRuntime.InvokeAsync<string>("localization.getCurrentLanguage");
            ResourceManager.CurrentLanguage = lang.StartsWith("ar") ? Language.Arabic : Language.English;
            IsRTL = ResourceManager.CurrentLanguage == Language.Arabic;
            await LoadListCategories();
            await LoadTreeCategories();
        }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                // Load resources 
                await ResourceLoaderService.LoadStyleSheet("css/treeView.css");
            }
        }
        // Helper method to get serial for sorting
        private int GetSerialForSorting(string? serial)
        {
            if (int.TryParse(serial, out int result))
                return result;
            return int.MaxValue; // Fallback for non-numeric values
        }
        // New method to move category up
        protected async Task MoveUp(CategoryDto category)
        {
            if (isMoving || !CanMoveUp(category))
                return;

            try
            {
                isMoving = true;
                var siblings = GetSiblingsAtSameLevel(category);
                var currentIndex = siblings.FindIndex(c => c.Id == category.Id);

                if (currentIndex <= 0)
                {
                    isMoving = false;
                    return;
                }

                var previousSibling = siblings[currentIndex - 1];
                await SwapSerials(category, previousSibling);
            }
            catch (Exception ex)
            {
                await JSRuntime.InvokeVoidAsync("swal",
                    ValidationResources.Error,
                    ex.Message,
                    "error");
            }
            finally
            {
                isMoving = false;
            }
        }
        // New method to move category down
        protected async Task MoveDown(CategoryDto category)
        {
            if (isMoving || !CanMoveDown(category))
                return;

            try
            {
                isMoving = true;
                var siblings = GetSiblingsAtSameLevel(category);
                var currentIndex = siblings.FindIndex(c => c.Id == category.Id);

                if (currentIndex < 0 || currentIndex >= siblings.Count - 1)
                {
                    isMoving = false;
                    return;
                }

                var nextSibling = siblings[currentIndex + 1];
                await SwapSerials(category, nextSibling);
            }
            catch (Exception ex)
            {
                await JSRuntime.InvokeVoidAsync("swal",
                    ValidationResources.Error,
                    ex.Message,
                    "error");
            }
            finally
            {
                isMoving = false;
            }
        }
        // Helper method to swap serials between two categories
        private async Task SwapSerials(CategoryDto category1, CategoryDto category2)
        {
            var assignments = new Dictionary<Guid, string>();

            // Swap serials
            assignments[category1.Id] = category2.TreeViewSerial?.ToString() ?? string.Empty;
            assignments[category2.Id] = category1.TreeViewSerial?.ToString() ?? string.Empty;

            // Update descendants if any
            UpdateDescendantSerials(category1, category1.TreeViewSerial?.ToString(),
                                  category2.TreeViewSerial?.ToString(), assignments);
            UpdateDescendantSerials(category2, category2.TreeViewSerial?.ToString(),
                                  category1.TreeViewSerial?.ToString(), assignments);

            // Call backend to update
            var result = await CategoryService.ReorderTreeAsync(assignments);
            if (!result.Success)
            {
                await JSRuntime.InvokeVoidAsync("swal",
                    ValidationResources.Error,
                    result.Message ?? "Failed to reorder category",
                    "error");
                return;
            }

            await LoadTreeCategories();
        }
        // Check if category can move up
        protected bool CanMoveUp(CategoryDto category)
        {
            var siblings = GetSiblingsAtSameLevel(category);
            var currentIndex = siblings.FindIndex(c => c.Id == category.Id);
            return currentIndex > 0;
        }

        // Check if category can move down
        protected bool CanMoveDown(CategoryDto category)
        {
            var siblings = GetSiblingsAtSameLevel(category);
            var currentIndex = siblings.FindIndex(c => c.Id == category.Id);
            return currentIndex >= 0 && currentIndex < siblings.Count - 1;
        }
        // Extract parent serial from category serial
        private string GetParentSerialFromSerial(string? serial)
        {
            if (string.IsNullOrEmpty(serial) || serial.Length <= 1)
                return string.Empty;
            return serial.Substring(0, serial.Length - 1);
        }
        // Get siblings at the same level
        private List<CategoryDto> GetSiblingsAtSameLevel(CategoryDto category)
        {
            // Use ParentId to find siblings instead of parsing serial numbers
            var categoriesList = TreeCategories.ToList();
            return categoriesList
                .Where(c => c.ParentId == category.ParentId)
                .OrderBy(c => GetSerialForSorting(c.TreeViewSerial))
                .ToList();
        }

        // Recursively update descendant serial numbers
        private void UpdateDescendantSerials(CategoryDto parent, string? oldParentSerial, string? newParentSerial,
            Dictionary<Guid, string> assignments)
        {
            if (string.IsNullOrEmpty(oldParentSerial)) return;
            var categoriesList = TreeCategories.ToList();
            var children = categoriesList
                .Where(c => c.ParentId == parent.Id)
                .OrderBy(c => GetSerialForSorting(c.TreeViewSerial))
                .ToList();
            foreach (var child in children)
            {
                var childSerial = child.TreeViewSerial?.ToString();
                if (!string.IsNullOrEmpty(childSerial) && childSerial.StartsWith(oldParentSerial))
                {
                    // Calculate new serial by replacing parent portion
                    var childSuffix = childSerial.Substring(oldParentSerial.Length);
                    var newChildSerial = newParentSerial + childSuffix;
                    // Add to assignments
                    assignments[child.Id] = newChildSerial;
                    // Recursively update grandchildren
                    UpdateDescendantSerials(child, childSerial, newChildSerial, assignments);
                }
            }
        }
        protected async Task Add()
        {
            Navigation.NavigateTo($"/category");
        }
        protected async Task Edit(CategoryDto item)
        {
            Navigation.NavigateTo($"/category/{item.Id}");
        }
        protected async Task LoadListCategories()
        {
            searchModel.PageSize = 10;
            await Search();
        }
        protected async Task LoadTreeCategories()
        {
            var result = await CategoryService.GetAllAsync();
            if (result.Success)
            {
                TreeCategories = result.Data.OrderBy(x => GetSerialForSorting(x.TreeViewSerial)) ?? Enumerable.Empty<CategoryDto>();
                await BuildTreeStructure();
            }
        }
        protected async Task Delete(Guid id)
        {
            // Check if the category has children
            var hasChildren = TreeCategories.Any(c => c.ParentId == id);
            if (hasChildren)
            {
                await JSRuntime.InvokeVoidAsync("swal",
                    ValidationResources.Error,
                    ValidationResources.CategoryHasChild,
                    "error");
                return;
            }
            var confirmed = await JSRuntime.InvokeAsync<bool>("swal", new
            {
                title = NotifiAndAlertsResources.AreYouSure,
                text = NotifiAndAlertsResources.ConfirmDeleteAlert,
                icon = "warning",
                buttons = new
                {
                    cancel = ActionsResources.Cancel,
                    confirm = ActionsResources.Confirm,
                },
                dangerMode = true,
            });
            if (confirmed)
            {
                var result = await CategoryService.DeleteAsync(id);
                if (result.Success)
                {
                    await JSRuntime.InvokeVoidAsync("swal",
                        ValidationResources.Done,
                        NotifiAndAlertsResources.DeletedSuccessfully,
                        "success");
                    if (isTreeView)
                    {
                        RemoveNodeFromTree(id);
                        await LoadTreeCategories();
                    }
                    else
                    {
                        await GoToPage(CurrentPage == TotalPages && TotalPages != 1 ?
                            CurrentPage - 1 : CurrentPage);
                        await LoadListCategories();
                    }
                }
                else
                {
                    if (result.Errors != null && result.Errors.Any())
                    {
                        await JSRuntime.InvokeVoidAsync("swal",
                            ValidationResources.Failed,
                            string.Join(", ", result.Errors),
                            "error");
                    }
                    else
                        await JSRuntime.InvokeVoidAsync("swal",
                        ValidationResources.Failed,
                        NotifiAndAlertsResources.DeleteFailed,
                        "error");
                }
            }
        }
        protected async Task Search()
        {
            try
            {
                var endpoint = ApiEndpoints.Category.Search;
                var result = await SearchService.SearchAsync(searchModel, endpoint);
                if (result.Success)
                {
                    ListCategories = result.Data?.Items ?? Enumerable.Empty<CategoryDto>();
                    TotalRecords = result.Data?.TotalRecords ?? 0;
                    TotalPages = (int)Math.Ceiling((double)TotalRecords / searchModel.PageSize);
                    CurrentPage = searchModel.PageNumber;
                }
            }
            catch (Exception)
            {
                await JSRuntime.InvokeVoidAsync("swal", ValidationResources.Error, NotifiAndAlertsResources.FailedToRetrieveData, "error");
            }
        }
        private async Task GoToPage(int page = 1)
        {
            if (page < 1 || page > TotalPages) return;
            CurrentPage = page;
            searchModel.PageNumber = page;
            await Search();
        }
        private void RemoveNodeFromTree(Guid nodeId)
        {
            if (expandedNodes.Contains(nodeId))
            {
                expandedNodes.Remove(nodeId);
            }
            RemoveNodeFromTreeRecursive(treeNodes, nodeId);
            StateHasChanged();
        }
        private bool RemoveNodeFromTreeRecursive(List<CategoryTreeNode> nodes, Guid nodeId)
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                if (nodes[i].Category.Id == nodeId)
                {
                    nodes.RemoveAt(i);
                    return true;
                }
                if (nodes[i].Children != null && nodes[i].Children.Any())
                {
                    if (RemoveNodeFromTreeRecursive(nodes[i].Children, nodeId))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        private async Task OnPageSizeChanged(ChangeEventArgs e)
        {
            if (int.TryParse(e.Value?.ToString(), out int newSize))
            {
                searchModel.PageSize = newSize;
                CurrentPage = 1;
                await GoToPage(CurrentPage);
            }
        }
        protected async Task ToggleView()
        {
            isTreeView = !isTreeView;
            if (isTreeView)
            {
                await LoadTreeCategories();
            }
            else
            {
                await LoadListCategories();
            }
        }
        private async Task BuildTreeStructure()
        {
            treeNodes.Clear();
            var categoriesList = TreeCategories.ToList();
            var rootCategories = categoriesList.Where(c =>
                c.IsFinal && (c.ParentId == Guid.Empty || c.ParentId == null)).ToList();
            foreach (var rootCategory in rootCategories.OrderBy(x => GetSerialForSorting(x.TreeViewSerial)))
            {
                var treeNode = new CategoryTreeNode
                {
                    Category = rootCategory,
                    Level = 0,
                    Children = BuildChildNodes(rootCategory, categoriesList, 1)
                };
                treeNodes.Add(treeNode);
            }
        }
        private List<CategoryTreeNode> BuildChildNodes(CategoryDto parent, List<CategoryDto> allCategories, int level)
        {
            var children = new List<CategoryTreeNode>();
            var childCategories = allCategories.Where(c => c.ParentId == parent.Id)
                                               .OrderBy(c => GetSerialForSorting(c.TreeViewSerial))
                                               .ToList();
            foreach (var child in childCategories)
            {
                var childNode = new CategoryTreeNode
                {
                    Category = child,
                    Level = level,
                    Children = BuildChildNodes(child, allCategories, level + 1)
                };
                children.Add(childNode);
            }
            return children;
        }
        protected bool HasChildren(CategoryTreeNode node)
        {
            return node.Children != null && node.Children.Any();
        }
        private void ExpandAll()
        {
            expandedNodes.Clear();
            if (treeNodes != null)
            {
                AddAllNodesToExpanded(treeNodes);
            }
            StateHasChanged();
        }
        private void CollapseAll()
        {
            expandedNodes.Clear();
            StateHasChanged();
        }
        private bool IsExpanded(Guid categoryId)
        {
            return expandedNodes.Contains(categoryId);
        }
        private void AddAllNodesToExpanded(IEnumerable<CategoryTreeNode> nodes)
        {
            foreach (var node in nodes)
            {
                if (HasChildren(node))
                {
                    expandedNodes.Add(node.Category.Id);
                    AddAllNodesToExpanded(node.Children!);
                }
            }
        }
        private void ToggleExpansion(Guid categoryId)
        {
            if (expandedNodes.Contains(categoryId))
            {
                expandedNodes.Remove(categoryId);
            }
            else
            {
                expandedNodes.Add(categoryId);
            }
            StateHasChanged();
        }
        // Helper class for tree structure
        public class CategoryTreeNode
        {
            public CategoryDto Category { get; set; } = null!;
            public int Level { get; set; }
            public List<CategoryTreeNode>? Children { get; set; } = new();
        }
    }
}