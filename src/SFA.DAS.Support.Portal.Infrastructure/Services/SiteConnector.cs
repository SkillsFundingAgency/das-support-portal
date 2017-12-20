using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SFA.DAS.Support.Portal.ApplicationServices.Services;

namespace SFA.DAS.Support.Portal.Infrastructure.Services
{
    public class SiteConnector : ISiteConnector
    {
        private readonly HttpClient _client;
        private const string TheHttpClientMustHaveAnAuthoriszationHeaderSupplied = "The Http Client must have an authoriszation header supplied";
        private const string TheHttpClientMayNotBeNull = "The Http client may not be null";
        public SiteConnector(HttpClient client)
        {
            _client = client ?? throw new ArgumentNullException( TheHttpClientMayNotBeNull);
            if (_client.DefaultRequestHeaders.Authorization == null)
            {
                throw new ArgumentException(TheHttpClientMustHaveAnAuthoriszationHeaderSupplied, nameof(_client.DefaultRequestHeaders.Authorization) );
            }
        }

        public async Task<T> Download<T>(string url) where T : class
        {
            return await Download<T>(new Uri(url));
        }

        public async Task<T> Upload<T>(Uri uri, IDictionary<string, string> formData)
        {
            var response = await _client.PostAsync(uri, new FormUrlEncodedContent(formData));
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(content);
        }

        public async Task<string> Download(string url)
        {
            var response = await _client.GetAsync(new Uri(url));
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return result;
        }

        public async Task<T> Download<T>(Uri uri) where T : class
        {
            var response = await _client.GetAsync(uri);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(content);
        }
    }
}
