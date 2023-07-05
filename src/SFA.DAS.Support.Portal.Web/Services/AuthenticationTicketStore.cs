using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.DataHandler.Serializer;
using SFA.DAS.Support.Portal.Web.Interfaces;
using SFA.DAS.Support.Portal.Web.Models.SFA.DAS.ProviderApprenticeshipsService.Domain.Models;
using System;
using System.Threading.Tasks;

namespace SFA.DAS.Support.Portal.Web.Services
{
    public class AuthenticationTicketStore : IAuthenticationSessionStore
    {
        private readonly DfESignInConfig _configuration;
        private readonly IDistributedCache _distributedCache;

        public AuthenticationTicketStore(IDistributedCache distributedCache, IDfESignInServiceConfiguration configuration)
        {
            _distributedCache = distributedCache;
            _configuration = configuration.DfEOidcConfiguration;
        }

        public async Task<string> StoreAsync(AuthenticationTicket ticket)
        {
            var key = Guid.NewGuid().ToString();
            await _distributedCache.SetAsync(key, new TicketSerializer().Serialize(ticket), new DistributedCacheEntryOptions
            {
                SlidingExpiration = TimeSpan.FromMinutes(_configuration.LoginSlidingExpiryTimeOutInMinutes)
            });
            return key;
        }

        public async Task RenewAsync(string key, AuthenticationTicket ticket)
        {
            await _distributedCache.RefreshAsync(key);
        }

        public async Task<AuthenticationTicket> RetrieveAsync(string key)
        {
            var result = await _distributedCache.GetAsync(key);
            return result == null ? null : new TicketSerializer().Deserialize(result);
        }

        public async Task RemoveAsync(string key)
        {
            await _distributedCache.RemoveAsync(key);
        }
    }
}