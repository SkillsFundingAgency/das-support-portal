using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace SFA.DAS.Support.Portal.Infrastructure.Settings
{
    [ExcludeFromCodeCoverage]
    public sealed class SiteConnectorSettings : ISiteConnectorSettings
    {
        //public string ClientId => ConfigurationManager.AppSettings["ClientId"];
        //public string AppKey => ConfigurationManager.AppSettings["AppKey"];
        //public string ResourceId => ConfigurationManager.AppSettings["ResourceId"];
        //public string Tenant => ConfigurationManager.AppSettings["Tenant"];

        [JsonRequired]
        public string ApiBaseUrl { get; set; }
        [JsonRequired]
        public string ClientId { get; set; }
        [JsonRequired]
        public string ClientSecret { get; set; }
        [JsonRequired]
        public string IdentifierUri { get; set; }
        [JsonRequired]
        public string Tenant { get; set; }
    }
}