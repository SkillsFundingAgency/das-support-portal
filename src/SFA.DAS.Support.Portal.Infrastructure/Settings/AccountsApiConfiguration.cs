using SFA.DAS.EAS.Account.Api.Client;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace SFA.DAS.Support.Portal.Infrastructure.Settings
{
    [ExcludeFromCodeCoverage]
    public class AccountsApiConfiguration : IAccountApiConfiguration
    {
        [JsonRequired]
        public string ApiBaseUrl { get; set; }
        [JsonRequired]
        public string ClientId { get; set;}
        [JsonRequired]
        public string ClientSecret { get; set;}
        [JsonRequired]
        public string IdentifierUri { get; set;}
        [JsonRequired]
        public string Tenant { get; set;}
    }
    
}
