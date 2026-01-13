using Common.Enumerations.Wallet;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Resources;
using Resources.Enumerations;
using Shared.DTOs.Wallet;
using Shared.GeneralModels.SearchCriteriaModels;
using Shared.Parameters.Commission;
using Shared.ResultModels.Commission;
using System.Text;
using UI.Contracts.Commission;
using UI.Contracts.General;

namespace UI.Pages.User.Marketer.Details.Components
{
    public partial class CommissionsLogsTab
    {
        protected CommissionsHistoryResult CommissionsHistoryResult { get; set; } = new();
        protected CommissionLogsFilterRequest CommissionLogsFilterRequest { get; set; } = new();
        protected IEnumerable<WalletCommissionsTransactionDto> CommissionLogs { get; set; } = Enumerable.Empty<WalletCommissionsTransactionDto>();
        protected Dictionary<string, Func<WalletCommissionsTransactionDto, object>> ExportColumns { get; } =
            new Dictionary<string, Func<WalletCommissionsTransactionDto, object>>
            {
                [ECommerceResources.Date] = x => x.CreatedDateLocalFormatted,
                [ECommerceResources.CommissionType] = x => x.CommissionType.ToString(),
                [ECommerceResources.Source] = x => x.Account.ToString(),
                [FormResources.Amount] = x => x.Amount,
                [FormResources.Status] = x => x.TransactionStatus.ToString()
            };
        protected BaseSearchCriteriaModel searchModel { get; set; } = new();
        protected int currentPage = 1;
        protected int totalRecords = 10;
        protected int totalPages = 1;

        [Inject] protected IJSRuntime JSRuntime { get; set; }
        [Inject] protected IResourceLoaderService ResourceLoaderService { get; set; }
        [Inject] protected ICommissionsHistoryService CommissionsHistoryService { get; set; }
        [Parameter] public Guid id { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            CommissionLogsFilterRequest.MarketerId = id;
            await SearchCommissionLogsAsync();
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
        protected virtual async Task OnCommissionTypeChanged(ChangeEventArgs e)
        {
            if (int.TryParse(e.Value?.ToString(), out int newType))
            {
                CommissionLogsFilterRequest.CommissionType = newType;
                await SearchCommissionLogsAsync();
            }
        }
        protected virtual async Task OnAccountPositionChanged(ChangeEventArgs e)
        {
            if (int.TryParse(e.Value?.ToString(), out int newPosition))
            {
                CommissionLogsFilterRequest.AccountPosition = newPosition;
                await SearchCommissionLogsAsync();
            }
        }
        #region Search and Pagination

        protected async Task SearchCommissionLogsAsync()
        {
            try
            {
                var result = await CommissionsHistoryService.SearchCommissionLogsAsync(searchModel, CommissionLogsFilterRequest);
                if (result.Success)
                {
                    CommissionLogs = result.Data.Items;
                    totalRecords = result.Data.TotalRecords;
                    totalPages = (int)Math.Ceiling((double)totalRecords / searchModel.PageSize);
                    currentPage = searchModel.PageNumber;
                    CommissionsHistoryResult = new CommissionsHistoryResult()
                    {
                        DirectSaleCommission = result.Data.Items.Where(x => x.CommissionType == WalletEarningType.DirectSale).Sum(x => x.Amount),
                        RecruitmentCommission = result.Data.Items.Where(x => x.CommissionType == WalletEarningType.Recruitment).Sum(x => x.Amount),
                        BinaryCommission = result.Data.Items.Where(x => x.CommissionType == WalletEarningType.Binary).Sum(x => x.Amount),
                        LevelCommission = result.Data.Items.Where(x => x.CommissionType == WalletEarningType.Level).Sum(x => x.Amount),
                        RankCommission = result.Data.Items.Where(x => x.CommissionType == WalletEarningType.Rank).Sum(x => x.Amount),
                    };
                    StateHasChanged();
                }
            }
            catch (Exception ex)
            {
                await JSRuntime.InvokeVoidAsync("swal", ValidationResources.Error, NotifiAndAlertsResources.FailedToRetrieveData, "error");
            }
        }

        protected virtual async Task GoToPage(int page = 1)
        {
            if (page < 1 || page > totalPages) return;

            currentPage = page;
            searchModel.PageNumber = page;
            await SearchCommissionLogsAsync();
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

        // Helper methods for pagination UI
        protected bool CanGoToPreviousPage => currentPage > 1;
        protected bool CanGoToNextPage => currentPage < totalPages;
        protected int StartRecord => (currentPage - 1) * searchModel.PageSize + 1;
        protected int EndRecord => Math.Min(currentPage * searchModel.PageSize, totalRecords);

        #endregion

        #region Export Functionality

        protected virtual async Task ExportToExcel(MouseEventArgs args)
        {
            await HandleExportToExcel(args, CommissionLogs, ECommerceResources.CommissionsLogs, ExportColumns, NotifiAndAlertsResources.NoDataToExport);
        }

        protected virtual async Task ExportToPrint(MouseEventArgs args)
        {
            var isRtl = ResourceManager.CurrentLanguage == Language.Arabic;
            await HandleExportToPrint(args, CommissionLogs, ECommerceResources.CommissionsLogs, ExportColumns, NotifiAndAlertsResources.NoDataToPrint, isRtl);
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
    }
}