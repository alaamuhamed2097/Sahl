using Dashboard.Configuration;
using Dashboard.Contracts.Authentication;
using Dashboard.Contracts.General;
using Dashboard.Models;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using Shared.GeneralModels;
using System.Text.Json;

namespace Dashboard.Services.General;

/// <summary>
/// API Service with automatic token refresh on 401 responses
/// </summary>
public class RefreshableApiService : IApiService
{
    private readonly IJSRuntime _jsRuntime;
    private readonly ITokenRefreshService _tokenRefresh;
    private readonly ITokenStorageService _tokenStorage;
    private readonly string _baseUrl;
    private readonly JsonSerializerOptions _jsonOptions;

    public RefreshableApiService(
        IJSRuntime jsRuntime,
        ITokenRefreshService tokenRefresh,
        ITokenStorageService tokenStorage,
        IOptions<ApiSettings> apiSettings)
    {
        _jsRuntime = jsRuntime;
        _tokenRefresh = tokenRefresh;
        _tokenStorage = tokenStorage;
        _baseUrl = apiSettings.Value.BaseUrl;

        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            WriteIndented = false
        };
    }

    public async Task<ResponseModel<T>> GetAsync<T>(
        string url,
        Dictionary<string, string>? headers = null)
    {
        return await ExecuteWithRetryAsync<T>(
            async () => await PerformRequestAsync<T>(url, "GET", null, headers)
        );
    }

    public async Task<ResponseModel<TResponse>> PostAsync<TRequest, TResponse>(
        string url,
        TRequest request,
        Dictionary<string, string>? headers = null,
        string contentType = "application/json")
    {
        return await ExecuteWithRetryAsync<TResponse>(
            async () => await PerformRequestAsync<TResponse>(url, "POST", request, headers)
        );
    }

    public async Task<ResponseModel<TResponse>> PutAsync<TRequest, TResponse>(
        string url,
        TRequest request,
        Dictionary<string, string>? headers = null,
        string contentType = "application/json")
    {
        return await ExecuteWithRetryAsync<TResponse>(
            async () => await PerformRequestAsync<TResponse>(url, "PUT", request, headers)
        );
    }

    public async Task<ResponseModel<T>> DeleteAsync<T>(
        string url,
        Dictionary<string, string>? headers = null)
    {
        return await ExecuteWithRetryAsync<T>(
            async () => await PerformRequestAsync<T>(url, "DELETE", null, headers)
        );
    }

    /// <summary>
    /// Core retry logic: If 401, refresh token and retry once
    /// </summary>
    private async Task<ResponseModel<T>> ExecuteWithRetryAsync<T>(
        Func<Task<ResponseModel<T>>> requestFunc)
    {
        var response = await requestFunc();

        // If 401 Unauthorized, try to refresh token and retry once
        if (response.StatusCode == 401)
        {
            Console.WriteLine("[RefreshableApiService] Got 401, attempting token refresh...");

            var refreshResult = await _tokenRefresh.RefreshTokenAsync();

            if (refreshResult.Success)
            {
                Console.WriteLine("[RefreshableApiService] Token refreshed successfully, retrying request...");

                // Retry the original request with new token
                response = await requestFunc();
            }
            else if (refreshResult.RequiresLogin)
            {
                Console.WriteLine("[RefreshableApiService] Refresh failed, redirecting to login...");

                // Clear tokens and redirect to login
                await _tokenStorage.ClearTokensAsync();
                await _jsRuntime.InvokeVoidAsync("location.assign", "/login");

                return new ResponseModel<T>
                {
                    Success = false,
                    Message = "Session expired. Please login again.",
                    StatusCode = 401
                };
            }
        }

        return response;
    }

    private async Task<ResponseModel<T>> PerformRequestAsync<T>(
        string url,
        string method,
        object? body = null,
        Dictionary<string, string>? headers = null)
    {
        try
        {
            var fullUrl = url.StartsWith("http") ? url : _baseUrl + url;

            var result = await _jsRuntime.InvokeAsync<FetchResponse>(
                "httpClientHelper.fetchWithCredentials",
                fullUrl,
                method,
                body,
                headers
            );

            if (!result.Ok)
            {
                return ConvertHttpError<T>(result);
            }

            try
            {
                var response = JsonSerializer.Deserialize<ResponseModel<T>>(result.Body, _jsonOptions);
                return response ?? new ResponseModel<T> { Success = false, Message = "Invalid response" };
            }
            catch (JsonException)
            {
                try
                {
                    var alt = JsonSerializer.Deserialize<ResponseModel<object>>(result.Body, _jsonOptions);
                    return new ResponseModel<T>
                    {
                        Success = alt?.Success ?? false,
                        Message = alt?.Message ?? string.Empty,
                        StatusCode = alt?.StatusCode ?? 200,
                        Errors = alt?.Errors ?? Enumerable.Empty<string>(),
                        Data = default
                    };
                }
                catch
                {
                    return new ResponseModel<T> { Success = false, Message = "Invalid response format" };
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[RefreshableApiService] {method} Error: {ex.Message}");
            return new ResponseModel<T> { Success = false, Message = ex.Message };
        }
    }

    private ResponseModel<T> ConvertHttpError<T>(FetchResponse response)
    {
        if (!string.IsNullOrWhiteSpace(response.Body))
        {
            try
            {
                var apiResponse = JsonSerializer.Deserialize<ResponseModel<T>>(response.Body, _jsonOptions);
                if (apiResponse != null)
                {
                    apiResponse.StatusCode = response.Status;
                    return apiResponse;
                }
            }
            catch { }
        }

        var errorMessage = response.Status switch
        {
            400 => "Invalid data was submitted",
            401 => "Unauthorized access, please log in again",
            403 => "You do not have permission to perform this action",
            404 => "The requested resource was not found",
            500 => "Server error, please try again later",
            _ => "An error occurred while processing your request"
        };

        if (!string.IsNullOrWhiteSpace(response.Body))
        {
            try
            {
                var errorObj = JsonSerializer.Deserialize<ApiErrorResponse>(response.Body, _jsonOptions);
                if (!string.IsNullOrWhiteSpace(errorObj?.Message))
                {
                    errorMessage = errorObj.Message;
                }
            }
            catch { }
        }

        return new ResponseModel<T>
        {
            Success = false,
            Message = errorMessage,
            StatusCode = response.Status,
            Errors = new List<string> { errorMessage }
        };
    }

    private class FetchResponse
    {
        public bool Ok { get; set; }
        public int Status { get; set; }
        public string Body { get; set; } = string.Empty;
    }
}