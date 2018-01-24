using SFA.DAS.Support.Common.Infrastucture.Settings;
using SFA.DAS.Support.Portal.ApplicationServices.Settings;
using SFA.DAS.Support.Portal.Infrastructure.Settings;
using SFA.DAS.Support.Shared.SiteConnection;

namespace SFA.DAS.Support.Portal.Web.Settings
{
    public interface IWebConfiguration
    {
        AuthSettings Authentication { get; set; }
        RoleSettings Roles { get; set; }
        ChallengeSettings Challenge { get; set; }
        SiteConnectorSettings SiteConnector { get; set; }
        SiteSettings Site { get; set; }
        ElasticSearchSettings ElasticSearch { get; set; }
        HmrcClientConfiguration HmrcClient { get; set; }
        AccountsApiConfiguration AccountsApi { get; set; }
        EmployerUsersApiConfiguration EmployerUsersApi { get; set; }
        LevySubmissionsApiConfiguration LevySubmissionsApi { get; set; }
    }
}