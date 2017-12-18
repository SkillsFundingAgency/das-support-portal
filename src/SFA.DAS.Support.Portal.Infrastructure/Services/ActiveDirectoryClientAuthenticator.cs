using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace SFA.DAS.Support.Portal.Infrastructure.Services
{
    [ExcludeFromCodeCoverage]
    internal class ActiveDirectoryClientAuthenticator : IActiveDirectoryClientAuthenticator
    {
        public async Task Authenticate(HttpClient client, string clientId, string appKey, string resourceId, string tenant)
        {
            if (client == null) throw new ArgumentNullException(nameof(client));
            if (clientId == null) throw new ArgumentNullException(nameof(clientId));
            if (appKey == null) throw new ArgumentNullException(nameof(appKey));
            if (resourceId == null) throw new ArgumentNullException(nameof(resourceId));
            if (tenant == null) throw new ArgumentNullException(nameof(tenant));
            if (client.DefaultRequestHeaders.Authorization != null) return ;
            var authority = $"https://login.microsoftonline.com/{tenant}";
            var clientCredential = new ClientCredential(clientId, appKey);
            var context = new AuthenticationContext(authority, true);
            var result = await context.AcquireTokenAsync(resourceId, clientCredential);
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", result.AccessToken);

        }
    }
}