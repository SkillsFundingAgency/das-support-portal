using System.Collections.Generic;
using SFA.DAS.Support.Shared.Authentication;
using SFA.DAS.Support.Shared.SearchIndexModel;

namespace SFA.DAS.Support.Shared.Discovery
{
    public class ServiceConfiguration
    {
        public Dictionary<SupportServiceIdentity, SiteManifest> ServiceManifests { get; set; }

        public ServiceConfiguration()
        {
            ServiceManifests = new Dictionary<SupportServiceIdentity, SiteManifest>
            {
                {
                    SupportServiceIdentity.SupportEmployerUser, new SiteManifest
                    {
                        ServiceIdentity = SupportServiceIdentity.SupportEmployerUser,
                        Resources = new[]
                        {
                            new SiteResource
                            {
                                ResourceKey = SupportServiceResourceKey.EmployerUser,
                                ResourceTitle = "Overview",

                                ResourceUrlFormat = "/user/index/{0}",
                                SearchItemsUrl = "/api/manifest/search",
                                SearchCategory = SearchCategory.User
                            },
                            new SiteResource
                            {
                                ResourceKey = SupportServiceResourceKey.EmployerUserHeader,
                                ResourceUrlFormat = "/user/header/{0}"
                            },
                            new SiteResource
                            {
                                ResourceKey = SupportServiceResourceKey.EmployerUserAccountTeam,
                                ResourceUrlFormat = "/account/team/{0}",
                                ResourceTitle = "Team members"
                            }
                        }
                    }
                },
                {
                    SupportServiceIdentity.SupportEmployerAccount, new SiteManifest
                    {
                        ServiceIdentity = SupportServiceIdentity.SupportEmployerAccount,
                        Resources = new[]
                        {
                            new SiteResource
                            {
                                ResourceKey = SupportServiceResourceKey.EmployerAccount,
                                ResourceUrlFormat = "/account/{0}",
                                ResourceTitle = "Organisations",
                                SearchItemsUrl = "/api/manifest/account",
                                SearchCategory = SearchCategory.Account
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
                                Challenge = "account/finance"
                            }
                        },
                        Challenges = new List<SiteChallenge>
                        {
                            new SiteChallenge
                            {
                                ChallengeKey = SupportServiceResourceKey.EmployerAccountFinance,
                                ChallengeUrlFormat = "/challenge/{0}"
                            }
                        }
                    }
                }
            };
        }
    }
}