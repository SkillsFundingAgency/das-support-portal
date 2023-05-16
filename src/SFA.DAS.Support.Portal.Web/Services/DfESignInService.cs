using Newtonsoft.Json;
using SFA.DAS.Support.Portal.Web.Interfaces;
using System.Net.Http;
using System.Threading.Tasks;

namespace SFA.DAS.Support.Portal.Web.Services
{
    /// <summary>
    /// Class responsible for DfESignIn Service.
    /// </summary>
    public class DfESignInService : IDfESignInService
    {
        private readonly HttpClient _httpClient;
        private readonly IDfESignInServiceConfiguration _config;

        public DfESignInService(HttpClient client, IDfESignInServiceConfiguration config)
        {
            _httpClient = client;
            _config = config;
        }

        /// <inheritdoc  />
        public async Task<T> Get<T>(string userId, string userOrgId) where T : class
        {
            var endpoint = GetEndPoint(userId, userOrgId);
            var request = new HttpRequestMessage(HttpMethod.Get, endpoint);
            var getResponse = await _httpClient.SendAsync(request);
            return getResponse.IsSuccessStatusCode ? JsonConvert.DeserializeObject<T>(await getResponse.Content.ReadAsStringAsync()) : null;
        }

        /// <summary>
        /// Method to generate the endpoint from configuration.
        /// </summary>
        /// <param name="userOrgId">User Org Id.</param>
        /// <param name="userId">User Id.</param>
        /// <returns>string.</returns>
        private string GetEndPoint(string userId, string userOrgId)
        {
            return $"{_config.DfEOidcConfiguration.ApiServiceUrl}/services/{_config.DfEOidcClientConfiguration.ApiServiceId}/organisations/{userOrgId}/users/{userId}";
        }
    }
}