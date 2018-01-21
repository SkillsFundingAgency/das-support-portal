using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SFA.DAS.NLog.Logger;

namespace SFA.DAS.Support.Shared.SiteConnection
{
    public class SiteConnector : ISiteConnector
    {
        private readonly HttpClient _client;
        private const string TheHttpClientMayNotBeNull = "The Http client may not be null";
        private readonly IClientAuthenticator _clientAuthenticator;
        private readonly ISiteConnectorSettings _settings;
        private readonly ILog _logger;
        public HttpStatusCode LastCode { get; set; }


        public SiteConnector(HttpClient client,
            IClientAuthenticator clientAuthenticator,
            ISiteConnectorSettings settings,
            ILog logger)
        {
            _client = client ?? throw new ArgumentNullException(TheHttpClientMayNotBeNull);
            _clientAuthenticator = clientAuthenticator ?? throw new ArgumentNullException(nameof(clientAuthenticator));
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        private async Task ExamineResponse(HttpResponseMessage message)
        {
            await Task.Run(() =>
            {
                try
                {
                    LastCode = HttpStatusCode.OK;
                    LastException = null;
                    message.EnsureSuccessStatusCode();

                }
                catch (HttpRequestException ex)
                {
                    LastCode = message.StatusCode;
                    LastException = ex;
                    switch (message.StatusCode)
                    {
                        case HttpStatusCode.Unauthorized:
                            _client.DefaultRequestHeaders.Authorization = null;
                            _logger.Error(ex,
                                $"Unexpected Http Status Code ({(int) message.StatusCode}) {message.StatusCode} during inter-site communication, changing token for retry");
                            break;
                        case HttpStatusCode.Forbidden:
                            _logger.Error(ex,
                                $"Unexpected Http Status Code ({(int) message.StatusCode}) {message.StatusCode} during inter-site communication, resource access denied");
                            break;
                        default:
                            _logger.Error(ex,
                                $"Unexpected Http Status Code ({(int) message.StatusCode}) {message.StatusCode} during inter-site communication");
                            throw;
                    }
                }
                catch (Exception ex)
                {
                    LastCode = message.StatusCode;
                    LastException = ex;
                    _logger.Error(ex, $"Unexpected exception during inter-site communication");
                    throw;
                }
            });

        }

        public Exception LastException { get; set; }


        public async Task<T> Download<T>(string url) where T : class
        {
            return await Download<T>(new Uri(url));
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

        public async Task<T> Upload<T>(Uri uri, IDictionary<string, string> formData) where T : class
        {
            await EnsureClientAuthorizationHeader();

            var response = await _client.PostAsync(uri, new FormUrlEncodedContent(formData));

            await ExamineResponse(response);

            if (LastCode == HttpStatusCode.Unauthorized || LastCode == HttpStatusCode.Forbidden) return null;

            var content = await response.Content.ReadAsStringAsync();

            if (typeof(T) == typeof(string)) return content as T;

            return JsonConvert.DeserializeObject<T>(content);

        }

        public async Task<string> Download(string url)
        {
            return await Download<string>(new Uri(url));

        }

        public async Task<T> Download<T>(Uri uri) where T : class
        {

            await EnsureClientAuthorizationHeader();

            var response = await _client.GetAsync(uri);

            await ExamineResponse(response);

            if (LastCode == HttpStatusCode.Unauthorized || LastCode == HttpStatusCode.Forbidden) return null;

            var content = await response.Content.ReadAsStringAsync();

            if (typeof(T) == typeof(string)) return content as T;

            return JsonConvert.DeserializeObject<T>(content);
        }
    }
}
