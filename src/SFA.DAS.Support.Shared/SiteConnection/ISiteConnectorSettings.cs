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

    public interface ISiteConnectorMISettings
    {
        SiteConnectorMI SupportCommitmentsSiteConnector { get; set; }

        SiteConnectorMI SupportEASSiteConnector { get; set; }

        SiteConnectorMI SupportEmployerUsersSiteConnector { get; set; }       
    }

    public class SiteConnectorMI
    {
        [JsonRequired] public string BaseUrl { get; set; }
        [JsonRequired] public string IdentifierUri { get; set; }        
    }
}