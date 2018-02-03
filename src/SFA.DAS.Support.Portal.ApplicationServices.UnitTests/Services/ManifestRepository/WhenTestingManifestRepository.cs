using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using SFA.DAS.NLog.Logger;
using SFA.DAS.Support.Portal.ApplicationServices.Services;
using SFA.DAS.Support.Portal.ApplicationServices.Settings;
using SFA.DAS.Support.Shared.Authentication;
using SFA.DAS.Support.Shared.Discovery;
using SFA.DAS.Support.Shared.SearchIndexModel;
using SFA.DAS.Support.Shared.SiteConnection;

namespace SFA.DAS.Support.Portal.ApplicationServices.UnitTests.Services.ManifestRepository
{
    public class WhenTestingManifestRepository
    {
        private Dictionary<SupportServiceResourceKey, SiteChallenge> _siteChallenges =
            new Dictionary<SupportServiceResourceKey, SiteChallenge>();

        private SupportServiceManifests _siteManifests = new SupportServiceManifests();

        private Dictionary<SupportServiceResourceKey, SiteResource> _siteResources =
            new Dictionary<SupportServiceResourceKey, SiteResource>();

        protected string HttpsTestsite;
        protected Mock<IFormMapper> MockFormMapper;
        protected Mock<ILog> MockLogger;
        protected Mock<ISiteConnector> MockSiteConnector;
        protected Mock<ISiteSettings> MockSiteSettings;

        protected SiteManifest TestSiteManifest;
        protected string TestSites;
        protected Uri TestSiteUri;

        protected IManifestRepository Unit;


        [SetUp]
        public virtual void Setup()
        {
            MockSiteSettings = new Mock<ISiteSettings>();
            MockSiteConnector = new Mock<ISiteConnector>();
            MockFormMapper = new Mock<IFormMapper>();
            MockLogger = new Mock<ILog>();

            _siteChallenges = new Dictionary<SupportServiceResourceKey, SiteChallenge>();
            _siteResources = new Dictionary<SupportServiceResourceKey, SiteResource>();
            _siteManifests = new SupportServiceManifests();


            HttpsTestsite = $"{SupportServiceIdentity.SupportEmployerAccount}|https://testsite/";

            TestSiteManifest = new SiteManifest
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
            };

            _siteManifests.Add(SupportServiceIdentity.SupportEmployerAccount, TestSiteManifest);


            TestSites = HttpsTestsite;

            MockSiteSettings.SetupGet(x => x.BaseUrls)
                .Returns(TestSites);

            TestSiteUri = TestSites.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries)
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x=>x.Split(new []{'|'},StringSplitOptions.RemoveEmptyEntries ))
                .Select(x => new Uri(x[1])).First();

            TestSiteUri = new Uri($"{TestSiteUri}api/manifest");

            MockSiteConnector.Setup(x => x.Download<SiteManifest>(TestSiteUri)).ReturnsAsync(TestSiteManifest);

            Unit = new ApplicationServices.Services.ManifestRepository(
                MockSiteSettings.Object,
                MockSiteConnector.Object,
                MockFormMapper.Object,
                MockLogger.Object, _siteManifests);
        }
    }
}