using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SFA.DAS.NLog.Logger;
using SFA.DAS.Support.Shared.Authentication;

namespace SFA.DAS.Support.Shared.SiteConnection
{
    public class SiteConnector : ISiteConnector
    {
        private const string TheHttpClientMayNotBeNull = "The Http client may not be null";
        private readonly HttpClient _client;
        private readonly IClientAuthenticator _clientAuthenticator;
        private readonly List<IHttpStatusCodeStrategy> _handlers;
        private readonly ILog _logger;
        private readonly ISiteConnectorSettings _settings;

        public SiteConnector(HttpClient client,
            IClientAuthenticator clientAuthenticator,
            ISiteConnectorSettings settings,
            List<IHttpStatusCodeStrategy> handlers,
            ILog logger)
        {
            _client = client ?? throw new ArgumentNullException(TheHttpClientMayNotBeNull);
            _clientAuthenticator = clientAuthenticator ?? throw new ArgumentNullException(nameof(clientAuthenticator));
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
            if (!handlers.Any()) throw new ArgumentException(nameof(handlers));
            _handlers = handlers;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public HttpStatusCode LastCode { get; set; }


        public string LastContent { get; set; }
        public Exception LastException { get; set; }
        public HttpStatusCodeDecision HttpStatusCodeDecision { get; set; }

        public async Task<T> Download<T>(string url) where T : class
        {
            return await Download<T>(new Uri(url));
        }

        public async Task<string> Download(string url)
        {
            return await Download<string>(new Uri(url));
        }

        public async Task<T> Upload<T>(Uri uri, IDictionary<string, string> formData) where T : class
        {
            await EnsureClientAuthorizationHeader();

            var response = await _client.PostAsync(uri, new FormUrlEncodedContent(formData));

            LastContent = await response.Content.ReadAsStringAsync();

            HttpStatusCodeDecision = ExamineResponse(response);

            switch (HttpStatusCodeDecision)
            {
                case HttpStatusCodeDecision.HandleException:
                    LastException = LastException ??
                                    new Exception($"An enforced exception has occured in {nameof(SiteConnector)}");
                    throw LastException;

                case HttpStatusCodeDecision.ReturnNull:
                    return null;

                default:
                    

                    if (typeof(T) == typeof(string)) return LastContent as T;

                    return JsonConvert.DeserializeObject<T>(LastContent);
            }
        }

        public async Task<T> Download<T>(Uri uri) where T : class
        {
            await EnsureClientAuthorizationHeader();

            var response = await _client.GetAsync(uri);

            LastContent = await response.Content.ReadAsStringAsync();

            HttpStatusCodeDecision = ExamineResponse(response);

            switch (HttpStatusCodeDecision)
            {
                case HttpStatusCodeDecision.RethrowException:
                    var exception = LastException ??
                                             new Exception($"A manufactured exception has occured in {nameof(SiteConnector)} after receiving status code {LastCode} and Content of: {LastContent}");
                    _logger.Error(exception, $"Forced Exception from {nameof(SiteConnector)} recieving {LastCode} and Content of: {LastContent} and Content of: {LastContent}");
                    throw exception;

                case HttpStatusCodeDecision.HandleException:
                    var generatedException = LastException ??
                          new Exception($"A manufactured exception has occured in {nameof(SiteConnector)} after receiving status code {LastCode}");
                    _logger.Error(generatedException, $"Exception from {nameof(SiteConnector)} recieving {LastCode} and Content of: {LastContent}");
                    return null;

                case HttpStatusCodeDecision.ReturnNull:
                    _logger.Debug($"Ignoring Return value from {nameof(SiteConnector)} receiving {LastCode} and Content of: {LastContent}");
                    return null;


                default:
                    _logger.Info($"Successful call to site from {nameof(SiteConnector)} recieving {LastCode}");
                    var content = await response.Content.ReadAsStringAsync();
                    if (typeof(T) == typeof(string)) return content as T;
                    return JsonConvert.DeserializeObject<T>(content);
            }
        }

        private async Task EnsureClientAuthorizationHeader()
        {
            if (_client.DefaultRequestHeaders.Authorization == null)
            {
                var token = await _clientAuthenticator.Authenticate(_settings.ClientId,
                    _settings.ClientSecret,
                    _settings.IdentifierUri,
                    _settings.Tenant);

                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }

        private HttpStatusCodeDecision ExamineResponse(HttpResponseMessage message)
        {
            LastException = null;
            LastCode = message.StatusCode;

            var handlerOptions = _handlers.Where(x => x.Low <= LastCode &&
                                                      x.High >= LastCode &&
                                                      x.ExcludeStatuses.All(y => y != LastCode)).ToList();

            if (handlerOptions.Any()) return handlerOptions.First().Handle(_client, LastCode);

            return HttpStatusCodeDecision.Continue;
        }
    }
}