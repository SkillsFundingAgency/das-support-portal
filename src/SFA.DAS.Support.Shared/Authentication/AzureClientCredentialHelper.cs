using System.Threading.Tasks;
using Microsoft.Azure.Services.AppAuthentication;

namespace SFA.DAS.Support.Shared.Authentication
{
    public class AzureClientCredentialHelper : IAzureClientCredentialHelper
    {
        public async Task<string> GetAccessTokenAsync(string identifier)
        {
            var azureServiceTokenProvider = new AzureServiceTokenProvider();
            var accessToken = await azureServiceTokenProvider.GetAccessTokenAsync(identifier);

            return accessToken;
        }
    }
}
