using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SFA.DAS.Support.Portal.ApplicationServices.Services;
using SFA.DAS.Support.Portal.Infrastructure.Settings;

namespace SFA.DAS.Support.Portal.Infrastructure.Services
{
    public class SiteConnector : ISiteConnector
    {
        private readonly HttpClient _client;
        private const string TheHttpClientMayNotBeNull = "The Http client may not be null";
        private const string TheClientAuthenticatorMayNotBeNull = "The client authenticator may not be null";
        private const string TheSettingsMayNotBeNull = "The settings may not be null";
        private readonly IActiveDirectoryClientAuthenticator _clientAuthenticator;
        private readonly ISiteConnectorSettings _settings;
        public SiteConnector(HttpClient client, 
                    IActiveDirectoryClientAuthenticator clientAuthenticator, ISiteConnectorSettings settings)
        {
            _client = client ?? throw new ArgumentException(TheHttpClientMayNotBeNull);
            _clientAuthenticator = clientAuthenticator ?? throw new ArgumentException(TheClientAuthenticatorMayNotBeNull);
            _settings = settings ?? throw new ArgumentException(TheSettingsMayNotBeNull);
        }

        public async Task<T> Download<T>(string url) where T : class
        {
            return await Download<T>(new Uri(url));
        }

        public async Task<T> Upload<T>(Uri uri, IDictionary<string, string> formData)
        {
            await _clientAuthenticator.Authenticate(_client, _settings.ClientId, _settings.AppKey, _settings.ResourceId,
                _settings.Tenant);
            var response = await _client.PostAsync(uri, new FormUrlEncodedContent(formData));
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(content);
        }

        public async Task<string> Download(string url)
        {
            await _clientAuthenticator.Authenticate(_client, _settings.ClientId, _settings.AppKey, _settings.ResourceId,
                _settings.Tenant);
            var response = await _client.GetAsync(new Uri(url));
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return result;
        }

        public async Task<T> Download<T>(Uri uri) where T : class
        {
            await _clientAuthenticator.Authenticate(_client, _settings.ClientId, _settings.AppKey, _settings.ResourceId,
                _settings.Tenant);
            var response = await _client.GetAsync(uri);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(content);

        }
    }
}
