namespace BL.Contracts.GeneralService
{
    public interface IApiService
    {
        Task<T> GetAsync<T>(string url, Dictionary<string, string> headers = null);
        Task<TResponse> PostAsync<TRequest, TResponse>(string url, TRequest request, Dictionary<string, string> headers = null);
    }
}
