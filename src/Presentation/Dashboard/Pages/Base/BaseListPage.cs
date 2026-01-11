using Common.Filters;
using Dashboard.Configuration;
using Dashboard.Contracts.General;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using Resources;
using Resources.Enumerations;
using Shared.GeneralModels;
using System.Text;

public abstract partial class BaseListPage<TDto> : ComponentBase, IDisposable
    where TDto : class
{
    private bool _scriptsLoaded = false;
    protected static string baseUrl = string.Empty;
    protected IEnumerable<TDto>? items;
    protected BaseSearchCriteriaModel searchModel { get; set; } = new();
    protected int currentPage = 1;
    protected int totalRecords = 10;
    protected int totalPages = 1;

    [Inject] protected ISearchService<TDto> SearchService { get; set; } = null!;
    [Inject] protected IJSRuntime JSRuntime { get; set; } = null!;
    [Inject] protected IResourceLoaderService ResourceLoaderService { get; set; } = null!;
    [Inject] protected NavigationManager Navigation { get; set; } = null!;
    [Inject] protected IOptions<ApiSettings> ApiOptions { get; set; } = default!;

    // Abstract properties and methods that derived classes must implement
    protected abstract string EntityName { get; }
    protected abstract string AddRoute { get; }
    protected abstract string EditRouteTemplate { get; }
    protected abstract string SearchEndpoint { get; }
    protected abstract Dictionary<string, Func<TDto, object>> ExportColumns { get; }

    // Abstract methods for CRUD operations
    protected abstract Task<ResponseModel<IEnumerable<TDto>>> GetAllItemsAsync();
    protected abstract Task<ResponseModel<bool>> DeleteItemAsync(Guid id);
    protected abstract Task<string> GetItemId(TDto item);

    protected override async Task OnInitializedAsync()
    {
        baseUrl = ApiOptions.Value.BaseUrl;
        await OnCustomInitializeAsync();
        searchModel.SearchTerm = "";
        await Search();
        await InvokeAsync(StateHasChanged);
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender && !_scriptsLoaded)
        {
            await ResourceLoaderService.LoadScriptsSequential(
                "Common/Downloader/fileDownloadHelper.js",
                "Common/Excel/excelExportHelper.js"
            );
            _scriptsLoaded = true;
        }
    }

    public virtual void Dispose()
    {
        ResourceLoaderService?.Dispose();
    }

    #region Data Loading

    protected virtual async Task LoadItems()
    {
        var result = await GetAllItemsAsync();
        if (result.Success)
            items = result.Data;
        else
            items = Enumerable.Empty<TDto>();

        StateHasChanged();
    }

	#endregion

	#region Navigation

	protected virtual void Add()
	{
		Navigation.NavigateTo(AddRoute); 
	}

	protected virtual async Task Edit(TDto item)
    {
        var id = await GetItemId(item);
        var editRoute = EditRouteTemplate.Replace("{id}", id.ToString());
        Navigation.NavigateTo(editRoute, true);
    }

    #endregion

    #region CRUD Operations

    protected virtual async Task Delete(Guid id)
    {
        var confirmed = await DeleteConfirmNotification();

        if (confirmed)
        {
            var result = await DeleteItemAsync(id);
            if (result.Success)
            {
                await ShowSuccessNotification(NotifiAndAlertsResources.DeletedSuccessfully);
                await Search();
                await OnAfterDeleteAsync(id);
                StateHasChanged();
            }
            else
            {
                if (result.Message == null)
                    await ShowErrorNotification(ValidationResources.Failed, NotifiAndAlertsResources.DeleteFailed);
                else
                    await ShowErrorNotification(ValidationResources.Failed, result.Message);
            }
        }
    }

    #endregion

    #region Search and Pagination

    protected virtual async Task Search()
    {
        try
        {
            await OnBeforeSearchAsync();

            var result = await SearchService.SearchAsync(searchModel, SearchEndpoint);
            if (result.Success)
            {
                items = result.Data.Items;
                totalRecords = result.Data.TotalRecords;
                totalPages = (int)Math.Ceiling((double)totalRecords / searchModel.PageSize);
                currentPage = searchModel.PageNumber;
                StateHasChanged();

                await OnAfterSearchAsync();
            }
        }
        catch (Exception)
        {
            await JSRuntime.InvokeVoidAsync("swal", ValidationResources.Error, NotifiAndAlertsResources.FailedToRetrieveData, "error");
        }
    }

    protected virtual async Task GoToPage(int page = 1)
    {
        if (page < 1 || page > totalPages) return;

        currentPage = page;
        searchModel.PageNumber = page;
        await Search();
    }

    protected virtual async Task OnPageSizeChanged(ChangeEventArgs e)
    {
        if (int.TryParse(e.Value?.ToString(), out int newSize))
        {
            searchModel.PageSize = newSize;
            currentPage = 1;
            await GoToPage(currentPage);
        }
    }

    /// <summary>
    /// Handle column sorting
    /// </summary>
    protected virtual async Task SortByColumn(string columnName)
    {
        if (searchModel.SortBy == columnName)
        {
            // Toggle sort direction if same column
            searchModel.SortDirection = searchModel.SortDirection == "asc" ? "desc" : "asc";
        }
        else
        {
            // New column, default to ascending
            searchModel.SortBy = columnName;
            searchModel.SortDirection = "asc";
        }

        // Reset to first page when sorting changes
        currentPage = 1;
        searchModel.PageNumber = 1;
        await Search();
    }

    /// <summary>
    /// Get CSS class for sort icon
    /// </summary>
    protected string GetSortIconClass(string columnName)
    {
        if (searchModel.SortBy != columnName)
            return "fas fa-sort text-muted";

        return searchModel.SortDirection == "asc"
            ? "fas fa-sort-up text-primary"
            : "fas fa-sort-down text-primary";
    }

    // Helper methods for pagination UI
    protected bool CanGoToPreviousPage => currentPage > 1;
    protected bool CanGoToNextPage => currentPage < totalPages;
    protected int StartRecord => (currentPage - 1) * searchModel.PageSize + 1;
    protected int EndRecord => Math.Min(currentPage * searchModel.PageSize, totalRecords);

    #endregion

    #region Export Functionality

    protected virtual async Task ExportToExcel(MouseEventArgs args)
    {
        await HandleExportToExcel(args, items, EntityName, ExportColumns, NotifiAndAlertsResources.NoDataToExport);
    }

    protected virtual async Task ExportToPrint(MouseEventArgs args)
    {
        var isRtl = ResourceManager.CurrentLanguage == Language.Arabic;
        await HandleExportToPrint(args, items, EntityName, ExportColumns, NotifiAndAlertsResources.NoDataToPrint, isRtl);
    }

    private async Task HandleExportToExcel<T>(MouseEventArgs args, IEnumerable<T> data, string sheetName,
        Dictionary<string, Func<T, object>> columns, string noDataMessage)
    {
        try
        {
            if (data == null || !data.Any())
            {
                await JSRuntime.InvokeVoidAsync("swal",
                    NotifiAndAlertsResources.Warning,
                    noDataMessage,
                    "warning");
                return;
            }

            // Prepare headers
            var headers = columns.Keys.ToArray();

            // Prepare data rows
            var dataRows = data.Select(item =>
                columns.Values.Select(func =>
                    func(item)?.ToString() ?? string.Empty
                ).ToArray()
            ).ToArray();

            // Determine if RTL
            var isRtl = ResourceManager.CurrentLanguage == Language.Arabic;

            // Generate filename
            var fileName = $"{sheetName}_{DateTime.Now:yyyyMMddHHmmss}.xlsx";

            // Call JavaScript export function
            var success = await JSRuntime.InvokeAsync<bool>(
                "excelExportHelper.exportToExcel",
                fileName,
                sheetName,
                headers,
                dataRows,
                isRtl
            );

            if (!success)
            {
                await JSRuntime.InvokeVoidAsync("swal",
                    ValidationResources.Error,
                    NotifiAndAlertsResources.ExportFailed,
                    "error");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Excel export error: {ex.Message}");
            await JSRuntime.InvokeVoidAsync("swal",
                ValidationResources.Error,
                NotifiAndAlertsResources.ExportFailed,
                "error");
        }
    }

    private async Task HandleExportToPrint<T>(MouseEventArgs args, IEnumerable<T> data, string reportTitle,
        Dictionary<string, Func<T, object>> columns, string noDataMessage, bool isRtl = false)
    {
        try
        {
            if (data == null || !data.Any())
            {
                await JSRuntime.InvokeVoidAsync("swal",
                    NotifiAndAlertsResources.Warning,
                    noDataMessage,
                    "warning");
                return;
            }

            var html = await GenerateHtmlReport(data, reportTitle, columns, isRtl);
            await JSRuntime.InvokeVoidAsync("printHtml", html.ToString());
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Print Error: {ex}");
            await JSRuntime.InvokeVoidAsync("swal",
                ValidationResources.Error,
                NotifiAndAlertsResources.PrintFailed,
                "error");
        }
    }

    public virtual async Task<string> GenerateHtmlReport<T>(IEnumerable<T> data, string title,
        Dictionary<string, Func<T, object>> columns, bool isRtl = false)
    {
        var html = new StringBuilder();
        html.AppendLine("<!DOCTYPE html>");
        html.AppendLine($"<html dir='{(isRtl ? "rtl" : "ltr")}'>");
        html.AppendLine("<head>");
        html.AppendLine("<meta charset='UTF-8'>");
        html.AppendLine($"<title>{title}</title>");
        html.AppendLine("<style>");
        html.AppendLine("@page { size: A4; margin: 1cm; }");
        html.AppendLine("body { font-family: Arial, sans-serif; margin: 0; padding: 10px; }");
        html.AppendLine("h1 { color: #333; text-align: center; margin-bottom: 20px; }");
        html.AppendLine(".report-info { text-align: left; margin-bottom: 20px; font-size: 0.9em; color: #666; }");
        html.AppendLine($".container {{ direction: {(isRtl ? "rtl" : "ltr")}; }}");
        html.AppendLine("table { width: 100%; border-collapse: collapse; margin-top: 10px; }");
        html.AppendLine("th { background-color: #4F81BD; color: white; padding: 8px; text-align: center; }");
        html.AppendLine("td { border: 1px solid #ddd; padding: 8px; text-align: center; }");
        html.AppendLine("tr:nth-child(even) { background-color: #f2f2f2; }");
        html.AppendLine(".footer { text-align: center; margin-top: 20px; font-size: 0.8em; color: #666; }");
        html.AppendLine("@media print { body { -webkit-print-color-adjust: exact; } }");
        html.AppendLine("</style>");
        html.AppendLine("</head>");
        html.AppendLine("<body>");
        html.AppendLine("<div class='container'>");
        html.AppendLine($"<h1>{title}</h1>");
        html.AppendLine($"<div>Generated on: {DateTime.Now:yyyy/MM/dd HH:mm}</div>");
        html.AppendLine("<div class='report-info'>");
        html.AppendLine("</div>");
        html.AppendLine("<table>");
        html.AppendLine("<thead><tr>");

        foreach (var column in columns)
        {
            html.AppendLine($"<th>{column.Key}</th>");
        }

        html.AppendLine("</tr></thead>");
        html.AppendLine("<tbody>");

        foreach (var item in data)
        {
            html.AppendLine("<tr>");
            foreach (var column in columns)
            {
                if (column.Key == FormResources.Image)
                    html.AppendLine($"<td><img src='{column.Value(item) ?? string.Empty}' class='rounded-circle' width='40' /></td>");
                else
                    html.AppendLine($"<td>{column.Value(item) ?? string.Empty}</td>");
            }
            html.AppendLine("</tr>");
        }

        html.AppendLine("</tbody></table>");
        html.AppendLine("<div class='footer'>Page 1</div>");
        html.AppendLine("</div>");
        html.AppendLine("</body></html>");

        return html.ToString();
    }

    #endregion

    #region Notifications

    protected virtual async Task ShowErrorNotification(string title, string message)
    {
        await JSRuntime.InvokeVoidAsync("swal", title, message, "error");
    }

    protected virtual async Task ShowWarningNotification(string title, string message)
    {
        await JSRuntime.InvokeVoidAsync("swal", title, message, "warning");
    }

    protected virtual async Task ShowSuccessNotification(string message)
    {
        await JSRuntime.InvokeVoidAsync("swal", ValidationResources.Done, message, "success");
    }

    protected virtual async Task<bool> DeleteConfirmNotification()
    {
        var options = new
        {
            title = NotifiAndAlertsResources.AreYouSure,
            text = NotifiAndAlertsResources.ConfirmDeleteAlert,
            icon = "warning",
            buttons = new
            {
                cancel = new
                {
                    text = ActionsResources.Cancel,
                    value = false,
                    visible = true,
                    className = "",
                    closeModal = true
                },
                confirm = new
                {
                    text = ActionsResources.Confirm,
                    value = true,
                    visible = true,
                    className = "swal-button--danger",
                    closeModal = true
                }
            }
        };

        return (await JSRuntime.InvokeAsync<bool>("swal", options));
    }

    protected virtual async Task<bool> ShowConfirmDialog(string title, string message, string confirmText = null, string cancelText = null)
    {
        var options = new
        {
            title = title,
            text = message,
            type = "warning", // v1 uses 'type' instead of 'icon'
            showCancelButton = true,
            confirmButtonText = confirmText ?? ActionsResources.Confirm,
            cancelButtonText = cancelText ?? ActionsResources.Cancel,
            closeOnConfirm = true,
            closeOnCancel = true
        };

        return (await JSRuntime.InvokeAsync<bool>("swal", options));
    }

    #endregion

    #region Virtual Methods for Customization

    /// <summary>
    /// Override this method to add custom initialization logic
    /// </summary>
    protected virtual async Task OnCustomInitializeAsync()
    {
        await Task.CompletedTask;
    }

    /// <summary>
    /// Override this method to add custom search logic
    /// </summary>
    protected virtual async Task OnBeforeSearchAsync()
    {
        await Task.CompletedTask;
    }

    /// <summary>
    /// Override this method to add custom logic after search
    /// </summary>
    protected virtual async Task OnAfterSearchAsync()
    {
        await Task.CompletedTask;
    }

    ///// <summary>
    ///// Override this method to customize the delete confirmation
    ///// </summary>
    protected virtual async Task<bool> OnBeforeDeleteAsync(Guid id)
    {
        return await Task.FromResult(true);
    }

    /// <summary>
    /// Override this method to add custom logic after successful delete
    /// </summary>
    protected virtual async Task OnAfterDeleteAsync(Guid id)
    {
        await Task.CompletedTask;
    }

    #endregion
}