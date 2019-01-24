using System.Collections.Generic;
using SFA.DAS.Support.Shared.Authentication;
using SFA.DAS.Support.Shared.SearchIndexModel;

namespace SFA.DAS.Support.Shared.Discovery
{
    public class EmployerAccountSiteManifest : SiteManifest
    {
        public EmployerAccountSiteManifest()
        {
            Resources = new[]
            {
                new SiteResource
                {
                    ServiceIdentity = SupportServiceIdentity.SupportEmployerAccount,
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
                    ServiceIdentity = SupportServiceIdentity.SupportEmployerAccount,
                    ResourceKey = SupportServiceResourceKey.EmployerAccountHeader,
                    ResourceUrlFormat = "/account/header/{0}"
                },
                new SiteResource
                {
                    ServiceIdentity = SupportServiceIdentity.SupportEmployerAccount,
                    ResourceKey = SupportServiceResourceKey.EmployerAccountFinance,
                    ResourceUrlFormat = "/account/finance/{0}",
                    ResourceTitle = "Finance",
                    Challenge = SupportServiceResourceKey.EmployerAccountFinanceChallenge,
                    HeaderKey = SupportServiceResourceKey.EmployerAccountHeader,
                    IsNavigationItem = true
                },
                  new SiteResource
                {
                    ServiceIdentity = SupportServiceIdentity.SupportEmployerAccount,
                    ResourceKey = SupportServiceResourceKey.EmployerAccountPayeSchemeLevys,
                    ResourceUrlFormat = "account/levysubmissions/{0}/{1}",
                    ResourceTitle = "Paye Scheme",
                    HeaderKey = SupportServiceResourceKey.EmployerAccountHeader,
                    Challenge = SupportServiceResourceKey.EmployerAccountFinanceChallenge
                },
                new SiteResource
                {
                    ServiceIdentity = SupportServiceIdentity.SupportEmployerAccount,
                    ResourceKey = SupportServiceResourceKey.EmployerAccountTeam,
                    ResourceUrlFormat = "/account/team/{0}",
                    ResourceTitle = "Team members",
                    HeaderKey = SupportServiceResourceKey.EmployerAccountHeader,
                    IsNavigationItem = true
                },
                new SiteResource
                {
                    ServiceIdentity = SupportServiceIdentity.SupportCommitments,
                    ResourceKey = SupportServiceResourceKey.CommitmentSearch,
                    ResourceUrlFormat = "/apprenticeships/search/{0}",
                    ResourceTitle = "Commitments",
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