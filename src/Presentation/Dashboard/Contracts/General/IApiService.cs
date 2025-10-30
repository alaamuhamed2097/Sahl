using Shared.GeneralModels;

namespace Dashboard.Contracts.General
{
    public interface IApiService
    {
        Task<ResponseModel<T>> GetAsync<T>(
            string url,
            Dictionary<string, string> headers = null);
        Task<ResponseModel<TResponse>> PostAsync<TRequest, TResponse>(
            string url,
            TRequest request,
            Dictionary<string, string> headers = null,
            string contentType = "application/json");
        Task<ResponseModel<TResponse>> PutAsync<TRequest, TResponse>(
            string url,
            TRequest request,
            Dictionary<string, string> headers = null,
            string contentType = "application/json");
        Task<ResponseModel<T>> DeleteAsync<T>(
            string url,
            Dictionary<string, string> headers = null);
    }
}
