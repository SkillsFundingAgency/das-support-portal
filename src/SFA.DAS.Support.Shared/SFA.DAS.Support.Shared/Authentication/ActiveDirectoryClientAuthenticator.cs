using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace SFA.DAS.Support.Shared.Authentication
{
    [ExcludeFromCodeCoverage]
    public class ActiveDirectoryClientAuthenticator : IClientAuthenticator
    {
        public async Task<string> Authenticate( string clientId, string appKey, string resourceId, string tenant)
        {
            if (clientId == null) throw new ArgumentNullException(nameof(clientId));
            if (appKey == null) throw new ArgumentNullException(nameof(appKey));
            if (resourceId == null) throw new ArgumentNullException(nameof(resourceId));
            if (tenant == null) throw new ArgumentNullException(nameof(tenant));

            var clientCredential = new ClientCredential(clientId, appKey);

            var authority = $"https://login.microsoftonline.com/{tenant}";
            var context = new AuthenticationContext(authority, true);

            var result = await context.AcquireTokenAsync(resourceId, clientCredential);

            return result.AccessToken;
        }
    }
}