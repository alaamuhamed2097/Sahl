using BL.Contracts.GeneralService.Notification;
using RestSharp;
using Shared.GeneralModels;
using Shared.GeneralModels.Parameters.Notification;

namespace BL.GeneralService.Notification;

public class MailGunProviderService : IEmailProviderService
{
    //private readonly IHttpClientFactory _httpClientFactory;
    //private readonly ILogger _logger;

    //private string _apiKey = "Helper.ItLegendAPIKey";
    //private string _baseUrl = "https://api.mailgun.net/v3";
    //private string _domain = "Helper.ItLegendDomain";
    //private string _displayName = "Helper.ItLegendDomain";
    //private string _fromEmail = "Helper.ItLegendDomain";

    //public MailGunProviderService(
    //    IHttpClientFactory httpClientFactory,
    //    ILogger logger)
    //{
    //    _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
    //    _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    //}

    //public RestResponse SendEmail(string toEmail, string subject, string emailTemplate)
    //{
    //    try
    //    {
    //        var httpClient = new HttpClient() { BaseAddress = new Uri("https://api.mailgun.net/v3") };
    //        var options = new RestClientOptions(httpClient.BaseAddress)
    //        {
    //            Authenticator = new HttpBasicAuthenticator("api", "Helper.ItLegendAPIKey")
    //        };
    //        RestClient client = new RestClient(httpClient, options);
    //        RestRequest request = new RestRequest();
    //        request.AddParameter("domain", "Helper.ItLegendDomain", ParameterType.UrlSegment);
    //        request.Resource = "{domain}/messages";
    //        request.AddParameter("from", $"{"Helper.sDisplayName"} <{"Helper.sWebsiteEmail"}>");
    //        request.AddParameter("to", toEmail);
    //        request.AddParameter("subject", subject);
    //        request.AddParameter("html", emailTemplate);
    //        request.Method = Method.Post;

    //        var result = client.Execute<ResponseModel<object>>(request);
    //        var url = result.ResponseUri;
    //        return result;
    //    }
    //    catch (Exception d)
    //    {
    //        return null;
    //    }
    //}

    //public ResponseModel<object> Send(EmailRequest request)
    //{
    //    if (request == null)
    //        throw new ArgumentNullException(nameof(request));

    //    try
    //    {
    //        using var httpClient = _httpClientFactory.CreateClient();
    //        httpClient.BaseAddress = new Uri(_baseUrl);

    //        var restClientOptions = new RestClientOptions(httpClient.BaseAddress)
    //        {
    //            Authenticator = new HttpBasicAuthenticator("api", _apiKey),
    //            ThrowOnAnyError = false
    //        };

    //        using var client = new RestClient(httpClient, restClientOptions);

    //        var restRequest = new RestRequest($"{_domain}/messages", Method.Post)
    //        {
    //            RequestFormat = DataFormat.Json
    //        };

    //        restRequest.AddParameter("from", $"{_displayName} <{_fromEmail}>");
    //        restRequest.AddParameter("to", request.To);
    //        restRequest.AddParameter("subject", request.Subject);
    //        restRequest.AddParameter(request.IsHtml ? "html" : "text", request.Body);

    //        var response = client.Execute(restRequest);

    //        if (!response.IsSuccessful)
    //        {
    //            _logger.Error($"Mailgun API request failed. Status: {response.StatusCode}, Content: {response.Content}");
    //            return new ResponseModel<object> { Success = false, Message = $"Mailgun API request failed. Status: {response.StatusCode}, Content: {response.Content}" };
    //        }

    //        return new ResponseModel<object> { Success = true, Message = $"Email successfully sent to {request.To}" };
    //    }
    //    catch (Exception ex)
    //    {
    //        throw new InvalidOperationException($"Failed to send email to {request.To}", ex);
    //    }
    //}
    public ResponseModel<object> Send(EmailRequest request)
    {
        throw new NotImplementedException();
    }

    public RestResponse SendEmail(string toEmail, string subject, string emailTemplate)
    {
        throw new NotImplementedException();
    }
}
