using System.Collections.Generic;
using SFA.DAS.Support.Shared.Authentication;
using SFA.DAS.Support.Shared.SearchIndexModel;

namespace SFA.DAS.Support.Shared.Discovery
{
    public class EmployerAccountSiteManifest : SiteManifest
    {
        public EmployerAccountSiteManifest()
        {
            ServiceIdentity = SupportServiceIdentity.SupportEmployerAccount;
            Resources = new[]
            {
                new SiteResource
                {
                    ResourceKey = SupportServiceResourceKey.EmployerAccount,
                    ResourceUrlFormat = "/account/{0}",
                    ResourceTitle = "Organisations",
                    SearchItemsUrl = "/api/search/accounts/{0}/{1}",
                    SearchTotalItemsUrl ="/api/search/accounts/totalCount/{0}",
                    SearchCategory = SearchCategory.Account,
                    HeaderKey = SupportServiceResourceKey.EmployerAccountHeader,
                    IsNavigationItem = true
                },
                new SiteResource
                {
                    ResourceKey = SupportServiceResourceKey.EmployerAccountHeader,
                    ResourceUrlFormat = "/account/header/{0}"
                },
                new SiteResource
                {
                    ResourceKey = SupportServiceResourceKey.EmployerAccountFinance,
                    ResourceUrlFormat = "/account/finance/{0}",
                    ResourceTitle = "Finance",
                    Challenge = SupportServiceResourceKey.EmployerAccountFinanceChallenge,
                    HeaderKey = SupportServiceResourceKey.EmployerAccountHeader,
                    IsNavigationItem = true
                },
                  new SiteResource
                {
                    ResourceKey = SupportServiceResourceKey.EmployerAccountPayeSchemeLevys,
                    ResourceUrlFormat = "account/levysubmissions/{0}/{1}",
                    ResourceTitle = "Paye Scheme",
                    HeaderKey = SupportServiceResourceKey.EmployerAccountHeader,
                },
                new SiteResource
                {
                    ResourceKey = SupportServiceResourceKey.EmployerAccountTeam,
                    ResourceUrlFormat = "/account/team/{0}",
                    ResourceTitle = "Team members",
                    HeaderKey = SupportServiceResourceKey.EmployerAccountHeader,
                    IsNavigationItem = true
                }
            };
            Challenges = new List<SiteChallenge>
            {
                new SiteChallenge
                {
                    ChallengeKey = SupportServiceResourceKey.EmployerAccountFinanceChallenge,
                    ChallengeUrlFormat = "/challenge/{0}"
                }
            };
        }
    }
}