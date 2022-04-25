using Newtonsoft.Json;

namespace SFA.DAS.Support.Shared.SiteConnection
{
    public interface ISiteConnectorSettings
    {
        string ClientId { get; set; }
        string ClientSecret { get; set; }
        string IdentifierUri { get; set; }
        string Tenant { get; set; }
    }

    public interface ISiteConnectorSettingsV2
    {
        SiteConnectorV2 SupportCommitmentsSiteConnector { get; set; }

        SiteConnectorV2 SupportEASSiteConnector { get; set; }

        SiteConnectorV2 SupportEmployerUsersSiteConnector { get; set; }       
    }

    public class SiteConnectorV2
    {
        [JsonRequired] public string BaseUrl { get; set; }
        [JsonRequired] public string IdentifierUri { get; set; }
        [JsonRequired] public string Tenant { get; set; }
    }
}