using Newtonsoft.Json;
using SFA.DAS.Support.Common.Infrastucture.Settings;
using SFA.DAS.Support.Portal.ApplicationServices.Settings;
using SFA.DAS.Support.Shared.SiteConnection;

namespace SFA.DAS.Support.Portal.Web.Settings
{
    public class WebConfiguration : IWebConfiguration
    {
        [JsonRequired] public AuthSettings Authentication { get; set; }
        [JsonRequired] public CryptoSettings Crypto { get; set; }
        [JsonRequired] public ChallengeSettings Challenge { get; set; }
        [JsonRequired] public ElasticSearchSettings ElasticSearch { get; set; }
        [JsonRequired] public RoleSettings Roles { get; set; }
        [JsonRequired] public SiteSettings Site { get; set; }
        [JsonRequired] public SiteConnectorSettings SiteConnector { get; set; }
        [JsonRequired] public EmployerAccountSiteConnectorSetting EmployerAccountSiteConnector { get; set; }

    }
}