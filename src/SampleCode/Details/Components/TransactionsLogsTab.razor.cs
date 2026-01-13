using Common.Enumerations.AccountType;
using Common.Enumerations.Wallet;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using Resources;
using Resources.Enumerations;
using Shared.DTOs.User.Marketer;
using Shared.DTOs.Wallet;
using Shared.GeneralModels.SearchCriteriaModels;
using System.Text;
using UI.Configuration;
using UI.Contracts.General;
using UI.Contracts.User.Marketer;
using UI.Contracts.Wallet;
using static UI.Constants.ApiEndpoints;

namespace UI.Pages.User.Marketer.Details.Components
{
    public partial class TransactionsLogsTab
    {
        private int CurrentPage = 1;
        private int TotalRecords = 0;
        private int TotalPages = 1;
        protected int PageSize { get; set; } = 10;
        [Parameter] public Guid id { get; set; }
        [Inject] protected IMarketerService MarketerService { get; set; } = null!;
        [Inject] protected IMarketerWalletTransactionService MarketerWalletService { get; set; } = null!;
        [Inject] private IJSRuntime JSRuntime { get; set; } = default!;
        [Inject] protected NavigationManager Navigation { get; set; } = null!;
        [Inject] protected IOptions<ApiSettings> ApiOptions { get; set; } = default!;
        [Inject] protected IResourceLoaderService ResourceLoaderService { get; set; }

        protected string baseUrl = string.Empty;
        protected MarketerDto Model { get; set; } = new();
        protected BaseSearchCriteriaModel searchModel { get; set; } = new();
        protected List<MarketerWalletTransactionsDto> WalletTransactions { get; set; } = new();
        protected List<MarketerWalletTransactionsDto> AllWalletTransactions { get; set; } = new(); // Store all transactions
        protected bool IsLoadingTransactions { get; set; } = false;
        private string searchTerm = string.Empty;
        private WalletTransactionType? selectedTransactionType = null;
        private AccountPosition? selectedAccount = null;

        protected override async Task OnParametersSetAsync()
        {
            baseUrl = ApiOptions.Value.BaseUrl;

            var result = await MarketerService.FindById(id);
            if (!result.Success || result.Data == null)
            {
                await JSRuntime.InvokeVoidAsync("swal", ValidationResources.Failed, NotifiAndAlertsResources.FailedToRetrieveData, "error");
                return;
            }

            Model = result.Data;

            await LoadWalletTransactions();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await ResourceLoaderService.LoadScriptsSequential(
                    "Common/Downloader/fileDownloadHelper.js",
                    "Common/Excel/excelExportHelper.js"
                );
            }
        }

        protected async Task LoadWalletTransactions()
        {
            searchModel.PageSize = PageSize;
            searchModel.PageNumber = CurrentPage;
            await SearchWalletTransactions();
        }

        protected async Task SearchWalletTransactions()
        {
            var endpoint = MarketerWalletTransactions.SearchWalletTransactions;
            Guid userId = Guid.Empty;
            Guid.TryParse(Model.ApplicationUserId, out userId);

            var result = await MarketerWalletService.SearchAsync(searchModel, userId);
            if (result.Success)
            {
                WalletTransactions = result.Data.Items.ToList();
                TotalRecords = result.Data.TotalRecords;
                TotalPages = (int)Math.Ceiling((double)TotalRecords / PageSize);
                StateHasChanged();
            }
            else
            {
                await JSRuntime.InvokeVoidAsync("swal",
                    ValidationResources.Error,
                    result.Message ?? ValidationResources.ErrorwhileGettingData,
                    "error");
            }
        }

        protected virtual async Task OnPageSizeChanged(ChangeEventArgs e)
        {
            if (int.TryParse(e.Value?.ToString(), out int newSize))
            {
                searchModel.PageSize = newSize;
                CurrentPage = 1;
                await GoToPage(CurrentPage);
            }
        }

        protected virtual async Task GoToPage(int page = 1)
        {
            if (page < 1 || page > TotalPages) return;

            CurrentPage = page;
            searchModel.PageNumber = page;
            await SearchWalletTransactions();
        }

        // Helper methods for pagination UI
        protected bool CanGoToPreviousPage => CurrentPage > 1;
        protected bool CanGoToNextPage => CurrentPage < TotalPages;
        protected int StartRecord => (CurrentPage - 1) * searchModel.PageSize + 1;
        protected int EndRecord => Math.Min(CurrentPage * searchModel.PageSize, TotalPages);

        #region Export Functionality

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
        protected decimal GetTotalWithdrawals() =>
            WalletTransactions
                .Where(t => t.TransactionType == WalletTransactionType.Withdraw && t.TransactionStatus == WalletTransactionStatus.Accepted || t.TransactionStatus == WalletTransactionStatus.Received && t.TransactionType == WalletTransactionType.Withdraw)
                .Sum(t => t.Amount);
        protected decimal GetTotalTransfers() =>
            WalletTransactions
                .Where(t => t.TransactionType == WalletTransactionType.Transfer)
                .Sum(t => t.Amount);

    }
}