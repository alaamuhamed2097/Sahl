using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using Shared.GeneralModels;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Dashboard.Configuration;
using Dashboard.Contracts.General;
using Dashboard.Contracts.Handlers;
using Dashboard.Models;

namespace Dashboard.Services.General
{
    public class ApiService : IApiService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;
        private readonly string _baseUrl;
        private readonly JsonSerializerOptions _jsonOptions;
        private readonly IApiStatusHandler _apiStatusHandler;

        public ApiService(
            HttpClient httpClient,
            ILocalStorageService localStorage,
            IOptions<ApiSettings> apiSettings,
            IApiStatusHandler apiStatusHandler)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _localStorage = localStorage ?? throw new ArgumentNullException(nameof(localStorage));
            _baseUrl = apiSettings.Value.BaseUrl;

            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                WriteIndented = false
            };
            _apiStatusHandler = apiStatusHandler;
        }

        private async Task AddBearerToken(HttpRequestMessage request)
        {
            if (await _localStorage.ContainKeyAsync("token"))
            {
                var token = await _localStorage.GetItemAsync<string>("token");
                if (!string.IsNullOrEmpty(token))
                {
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }
            }
        }

        private async Task<ResponseModel<T>> ConvertApiExceptions<T>(HttpResponseMessage response)
        {
            try
            {
                var errorResponse = await response.Content.ReadAsStringAsync();
                var errorMessage = response.StatusCode switch
                {
                    HttpStatusCode.BadRequest => "Invalid data was submitted",
                    HttpStatusCode.Unauthorized => "Unauthorized access, please log in again",
                    HttpStatusCode.Forbidden => "You do not have permission to perform this action",
                    HttpStatusCode.NotFound => "The requested resource was not found",
                    HttpStatusCode.InternalServerError => "Server error, please try again later",
                    _ => "An error occurred while processing your request"
                };

                // Try to extract server error details if available
                if (!string.IsNullOrWhiteSpace(errorResponse))
                {
                    try
                    {
                        var errorObj = JsonSerializer.Deserialize<ApiErrorResponse>(errorResponse, _jsonOptions);
                        if (!string.IsNullOrWhiteSpace(errorObj?.Message))
                        {
                            errorMessage = errorObj.Message;
                        }
                    }
                    catch { /* Ignore deserialization errors */ }
                }

                return new ResponseModel<T>
                {
                    Message = "Failed to process error response",
                    Errors = new List<string> { "Failed to process error response" },
                    StatusCode = (int)response.StatusCode,
                    Success = false
                };
            }
            catch
            {
                return new ResponseModel<T>
                {
                    Message = "Failed to process error response",
                    Success = false
                };
            }
        }

        public async Task<ResponseModel<T>> GetAsync<T>(
            string url,
            Dictionary<string, string> headers = null)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, _baseUrl + url);
                await AddBearerToken(request);

                if (headers != null)
                {
                    foreach (var header in headers)
                    {
                        request.Headers.Add(header.Key, header.Value);
                    }
                }

                var response = await _httpClient.SendAsync(request);
                if (!response.IsSuccessStatusCode)
                {
                    if(response.StatusCode == HttpStatusCode.Unauthorized)
                        _apiStatusHandler.HandleUnAuthorizeStatusAsync();
                    return await ConvertApiExceptions<T>(response);
                }

                var jsonResponse = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<ResponseModel<T>>(jsonResponse, _jsonOptions);

                // Validate the deserialized response
                if (result == null)
                {
                    return new ResponseModel<T>
                    {
                        Message = "Invalid API response format",
                        Success = false
                    };
                }

                return result;
            }
            catch (TaskCanceledException)
            {
                return new ResponseModel<T>
                {
                    Message = "Request was canceled",
                    Success = false
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel<T>
                {
                    Message = $"Unexpected error: {ex.Message}",
                    Success = false
                };
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
                var httpRequest = new HttpRequestMessage(HttpMethod.Post, _baseUrl + url);
                await AddBearerToken(httpRequest);

                if (headers != null)
                {
                    foreach (var header in headers)
                    {
                        httpRequest.Headers.Add(header.Key, header.Value);
                    }
                }

                var jsonRequest = JsonSerializer.Serialize(request);
                httpRequest.Content = new StringContent(jsonRequest, Encoding.UTF8, contentType);

                var response = await _httpClient.SendAsync(httpRequest);
                if (!response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                        _apiStatusHandler.HandleUnAuthorizeStatusAsync();
                    return await ConvertApiExceptions<TResponse>(response);
                }

                var jsonResponse = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<ResponseModel<TResponse>>(jsonResponse, _jsonOptions);

                // Validate the deserialized response
                if (result == null)
                {
                    return new ResponseModel<TResponse>
                    {
                        Message = "Invalid API response format",
                        Success = false
                    };
                }

                return result;
            }
            catch (TaskCanceledException)
            {
                return new ResponseModel<TResponse>
                {
                    Message = "Request was canceled",
                    Success = false
                };
            }
            catch (JsonException jsonEx)
            {
                return new ResponseModel<TResponse>
                {
                    Message = $"Failed to process JSON: {jsonEx.Message}  omar",
                    Success = false
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel<TResponse>
                {
                    Message = $"Unexpected error: {ex.Message} roka",
                    Success = false
                };
            }
        }
        public async Task<ResponseModel<TResponse>> PostFormAsync<TRequest, TResponse>(
    string url,
    TRequest request,
    Dictionary<string, string> headers = null)
        {
            try
            {
                var httpRequest = new HttpRequestMessage(HttpMethod.Post, _baseUrl + url);
                await AddBearerToken(httpRequest);

                if (headers != null)
                {
                    foreach (var header in headers)
                        httpRequest.Headers.Add(header.Key, header.Value);
                }

                // Convert object to key-value form data
                var formData = request!.GetType()
                    .GetProperties()
                    .ToDictionary(
                        p => p.Name,
                        p => p.GetValue(request)?.ToString() ?? string.Empty
                    );

                httpRequest.Content = new FormUrlEncodedContent(formData);

                var response = await _httpClient.SendAsync(httpRequest);

                if (!response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                        _apiStatusHandler.HandleUnAuthorizeStatusAsync();
                    return await ConvertApiExceptions<TResponse>(response);
                }
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<ResponseModel<TResponse>>(jsonResponse, _jsonOptions);

                return result ?? new ResponseModel<TResponse>
                {
                    Success = false,
                    Message = "Invalid API response format"
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel<TResponse>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }
        public async Task<ResponseModel<TResponse>> PostMultipartAsync<TRequest, TResponse>(
            string url,
            TRequest request,
            Dictionary<string, string> headers = null)
        {
            try
            {
                var httpRequest = new HttpRequestMessage(HttpMethod.Post, _baseUrl + url);
                await AddBearerToken(httpRequest);

                if (headers != null)
                {
                    foreach (var header in headers)
                        httpRequest.Headers.Add(header.Key, header.Value);
                }

                var form = new MultipartFormDataContent();

                foreach (var prop in request!.GetType().GetProperties())
                {
                    var value = prop.GetValue(request);
                    if (value == null) continue;

                    if (value is Stream stream) // file
                    {
                        form.Add(new StreamContent(stream), prop.Name, "file.jpg");
                    }
                    else
                    {
                        form.Add(new StringContent(value.ToString()!), prop.Name);
                    }
                }

                httpRequest.Content = form;

                var response = await _httpClient.SendAsync(httpRequest);

                if (!response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                        _apiStatusHandler.HandleUnAuthorizeStatusAsync();
                    return await ConvertApiExceptions<TResponse>(response);
                }
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<ResponseModel<TResponse>>(jsonResponse, _jsonOptions);

                return result ?? new ResponseModel<TResponse>
                {
                    Success = false,
                    Message = "Invalid API response format"
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel<TResponse>
                {
                    Success = false,
                    Message = ex.Message
                };
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
                var httpRequest = new HttpRequestMessage(HttpMethod.Put, _baseUrl + url);
                await AddBearerToken(httpRequest);

                if (headers != null)
                {
                    foreach (var header in headers)
                    {
                        httpRequest.Headers.Add(header.Key, header.Value);
                    }
                }

                var jsonRequest = JsonSerializer.Serialize(request);
                httpRequest.Content = new StringContent(jsonRequest, Encoding.UTF8, contentType);

                var response = await _httpClient.SendAsync(httpRequest);
                if (!response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                        _apiStatusHandler.HandleUnAuthorizeStatusAsync();
                    return await ConvertApiExceptions<TResponse>(response);
                }

                var jsonResponse = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<ResponseModel<TResponse>>(jsonResponse, _jsonOptions);

                // Validate the deserialized response
                if (result == null)
                {
                    return new ResponseModel<TResponse>
                    {
                        Message = "Invalid API response format",
                        Success = false
                    };
                }

                return result;
            }
            catch (TaskCanceledException)
            {
                return new ResponseModel<TResponse>
                {
                    Message = "Request was canceled",
                    Success = false
                };
            }
            catch (JsonException jsonEx)
            {
                return new ResponseModel<TResponse>
                {
                    Message = $"Failed to process JSON: {jsonEx.Message}",
                    Success = false
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel<TResponse>
                {
                    Message = $"Unexpected error: {ex.Message}",
                    Success = false
                };
            }
        }

    }
}
