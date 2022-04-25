using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace SFA.DAS.Support.Shared.SiteConnection
{
    [ExcludeFromCodeCoverage]
    public sealed class SiteConnectorSettings : ISiteConnectorSettings
    {
        [JsonRequired] public string ClientId { get; set; }

        [JsonRequired] public string ClientSecret { get; set; }

        [JsonRequired] public string IdentifierUri { get; set; }

        [JsonRequired] public string Tenant { get; set; }
    }


    [ExcludeFromCodeCoverage]
    public sealed class SiteConnectorSettingsV2 : ISiteConnectorSettingsV2
    {
        public SiteConnectorV2 SupportCommitmentsSiteConnector { get; set; }
        public SiteConnectorV2 SupportEASSiteConnector { get; set; }
        public SiteConnectorV2 SupportEmployerUsersSiteConnector { get; set; }
    }
}