using Dashboard.Configuration;
using Dashboard.Contracts.General;
using Dashboard.Models;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using Shared.GeneralModels;
using System.Text.Json;

namespace Dashboard.Services.General
{
    /// <summary>
    /// Cookie-based API service using JavaScript fetch for Blazor WebAssembly.
    /// CRITICAL: HttpClient in WASM doesn't automatically send cookies, so we use fetch with credentials: 'include'.
    /// </summary>
    public class CookieApiService : IApiService
    {
        private readonly IJSRuntime _jsRuntime;
        private readonly string _baseUrl;
        private readonly JsonSerializerOptions _jsonOptions;

        public CookieApiService(
            IJSRuntime jsRuntime,
            IOptions<ApiSettings> apiSettings)
        {
            _jsRuntime = jsRuntime ?? throw new ArgumentNullException(nameof(jsRuntime));
            _baseUrl = apiSettings.Value.BaseUrl;

            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                WriteIndented = false
            };
        }

        public async Task<ResponseModel<T>> GetAsync<T>(
            string url,
            Dictionary<string, string> headers = null)
        {
            try
            {
                var fullUrl = url.StartsWith("http") ? url : _baseUrl + url;

                var result = await _jsRuntime.InvokeAsync<FetchResponse>(
                    "httpClientHelper.fetchWithCredentials",
                    fullUrl,
                    "GET",
                    null,
                    headers
                );

                if (!result.Ok)
                {
                    return ConvertHttpError<T>(result);
                }

                // Try to deserialize to expected generic type, fallback to loose mapping when server uses different generic
                try
                {
                    var response = JsonSerializer.Deserialize<ResponseModel<T>>(result.Body, _jsonOptions);
                    return response ?? new ResponseModel<T> { Success = false, Message = "Invalid response" };
                }
                catch (JsonException)
                {
                    // Fallback: parse as ResponseModel<object> and map
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
                Console.WriteLine($"GET Error: {ex.Message}");
                return new ResponseModel<T> { Success = false, Message = ex.Message };
            }
        }

        public async Task<ResponseModel<TResponse>> PostAsync<TRequest, TResponse>(
            string url,
            TRequest request,
            Dictionary<string, string> headers = null,
            string contentType = "application/json")
        {
            try
            {
                var fullUrl = url.StartsWith("http") ? url : _baseUrl + url;

                var result = await _jsRuntime.InvokeAsync<FetchResponse>(
                    "httpClientHelper.fetchWithCredentials",
                    fullUrl,
                    "POST",
                    request,
                    headers
                );

                if (!result.Ok)
                {
                    return ConvertHttpError<TResponse>(result);
                }

                try
                {
                    var response = JsonSerializer.Deserialize<ResponseModel<TResponse>>(result.Body, _jsonOptions);
                    return response ?? new ResponseModel<TResponse> { Success = false, Message = "Invalid response" };
                }
                catch (JsonException)
                {
                    // Fallback: server returned different generic type (e.g. ResponseModel<string>) — map its Success/Message and keep Data default
                    try
                    {
                        var alt = JsonSerializer.Deserialize<ResponseModel<object>>(result.Body, _jsonOptions);
                        return new ResponseModel<TResponse>
                        {
                            Success = alt?.Success ?? false,
                            Message = alt?.Message ?? string.Empty,
                            StatusCode = alt?.StatusCode ?? 200,
                            Errors = alt?.Errors ?? Enumerable.Empty<string>(),
                            Data = default
                        };
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"POST Deserialize fallback failed: {ex.Message}");
                        return new ResponseModel<TResponse> { Success = false, Message = "Invalid response format" };
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"POST Error: {ex.Message}");
                return new ResponseModel<TResponse> { Success = false, Message = ex.Message };
            }
        }

        public async Task<ResponseModel<TResponse>> PutAsync<TRequest, TResponse>(
            string url,
            TRequest request,
            Dictionary<string, string> headers = null,
            string contentType = "application/json")
        {
            try
            {
                var fullUrl = url.StartsWith("http") ? url : _baseUrl + url;

                var result = await _jsRuntime.InvokeAsync<FetchResponse>(
                    "httpClientHelper.fetchWithCredentials",
                    fullUrl,
                    "PUT",
                    request,
                    headers
                );

                if (!result.Ok)
                {
                    return ConvertHttpError<TResponse>(result);
                }

                try
                {
                    var response = JsonSerializer.Deserialize<ResponseModel<TResponse>>(result.Body, _jsonOptions);
                    return response ?? new ResponseModel<TResponse> { Success = false, Message = "Invalid response" };
                }
                catch (JsonException)
                {
                    try
                    {
                        var alt = JsonSerializer.Deserialize<ResponseModel<object>>(result.Body, _jsonOptions);
                        return new ResponseModel<TResponse>
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
                        return new ResponseModel<TResponse> { Success = false, Message = "Invalid response format" };
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"PUT Error: {ex.Message}");
                return new ResponseModel<TResponse> { Success = false, Message = ex.Message };
            }
        }

        public async Task<ResponseModel<T>> DeleteAsync<T>(
            string url,
            Dictionary<string, string> headers = null)
        {
            try
            {
                var fullUrl = url.StartsWith("http") ? url : _baseUrl + url;

                var result = await _jsRuntime.InvokeAsync<FetchResponse>(
                    "httpClientHelper.fetchWithCredentials",
                    fullUrl,
                    "DELETE",
                    null,
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
                Console.WriteLine($"DELETE Error: {ex.Message}");
                return new ResponseModel<T> { Success = false, Message = ex.Message };
            }
        }

        private ResponseModel<T> ConvertHttpError<T>(FetchResponse response)
        {
            // First try to parse ResponseModel<T> returned by the API (detailed errors)
            if (!string.IsNullOrWhiteSpace(response.Body))
            {
                try
                {
                    var apiResponse = JsonSerializer.Deserialize<ResponseModel<T>>(response.Body, _jsonOptions);
                    if (apiResponse != null)
                    {
                        // Ensure status code is carried
                        apiResponse.StatusCode = response.Status;
                        return apiResponse;
                    }
                }
                catch { /* ignore */ }
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

            // Try to parse ApiErrorResponse or include raw body for debugging
            if (!string.IsNullOrWhiteSpace(response.Body))
            {
                try
                {
                    var errorObj = JsonSerializer.Deserialize<ApiErrorResponse>(response.Body, _jsonOptions);
                    if (!string.IsNullOrWhiteSpace(errorObj?.Message))
                    {
                        errorMessage = errorObj.Message;
                    }
                    else
                    {
                        // include raw body if parsing didn't yield a message
                        errorMessage = errorMessage + " | " + response.Body;
                    }
                }
                catch
                {
                    // include raw body when parsing fails
                    errorMessage = errorMessage + " | " + response.Body;
                }
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
            public string StatusText { get; set; } = string.Empty;
            public string Body { get; set; } = string.Empty;
        }
    }
}
