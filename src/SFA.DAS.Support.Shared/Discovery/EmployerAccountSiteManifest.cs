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
                    SearchTotalItemsUrl = "/api/search/accounts/totalCount/{0}",
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
                    IsNavigationItem = true,
                },
                new SiteResource
                {
                    ServiceIdentity = SupportServiceIdentity.SupportEmployerAccount,
                    ResourceKey = SupportServiceResourceKey.EmployerAccountChangeRole,
                    ResourceUrlFormat = "/role/{0}/change/{1}",
                    ResourceTitle = "Role",
                    HeaderKey = SupportServiceResourceKey.EmployerAccountChangeRoleHeader,
                    IsNavigationItem = false
                },
                new SiteResource
                {
                    ServiceIdentity = SupportServiceIdentity.SupportEmployerAccount,
                    ResourceKey = SupportServiceResourceKey.EmployerAccountResendInvitation,
                    ResourceUrlFormat = "/account/{0}/resend-invitation/{1}",
                    HeaderKey = SupportServiceResourceKey.EmployerAccountHeader,
                    IsNavigationItem = false
                },
                new SiteResource
                {
                    ServiceIdentity = SupportServiceIdentity.SupportCommitments,
                    ResourceKey = SupportServiceResourceKey.CommitmentSearch,
                    ResourceUrlFormat = "/apprenticeships/search/{0}",
                    SearchItemsUrl = "/apprenticeships/search?hashedAccountId={0}&searchType={1}&searchTerm={2}",
                    ResourceTitle = "Commitments",
                    HeaderKey = SupportServiceResourceKey.EmployerAccountHeader,
                    IsNavigationItem = true
                },
                new SiteResource
                {
                    ServiceIdentity = SupportServiceIdentity.SupportCommitments,
                    ResourceKey = SupportServiceResourceKey.CommitmentApprenticeDetail,
                    ResourceUrlFormat = "/Apprenticeships/account/{0}/Apprenticeship/{1}",
                    ResourceTitle = string.Empty,
                    HeaderKey = SupportServiceResourceKey.EmployerAccountHeader,
                    IsNavigationItem = false
                },
                new SiteResource
                {
                    ServiceIdentity = SupportServiceIdentity.SupportCommitments,
                    ResourceKey = SupportServiceResourceKey.CommitmentCohortDetail,
                    ResourceUrlFormat = "/Apprenticeships/account/{0}/cohort/{1}",
                    ResourceTitle = string.Empty,
                    HeaderKey = SupportServiceResourceKey.EmployerAccountHeader,
                    IsNavigationItem = false
                }
            };
            Challenges = new List<SiteChallenge>
            {
                new SiteChallenge
                {
                    ServiceIdentity = SupportServiceIdentity.SupportEmployerAccount,
                    ChallengeKey = SupportServiceResourceKey.EmployerAccountFinanceChallenge,
                    ChallengeUrlFormat = "/challenge/{0}"
                }
            };
        }
    }
}