using SFA.DAS.Support.Common.Infrastucture.Settings;
using SFA.DAS.Support.Portal.ApplicationServices.Settings;
using SFA.DAS.Support.Shared.SiteConnection;

namespace SFA.DAS.Support.Portal.Web.Settings
{
    public interface IWebConfiguration
    {
        AuthSettings Authentication { get; set; }
        CryptoSettings Crypto { get; set; }
        ChallengeSettings Challenge { get; set; }
        ElasticSearchSettings ElasticSearch { get; set; }
        RoleSettings Roles { get; set; }
        SiteConnectorSettings SiteConnector { get; set; }
    }
}