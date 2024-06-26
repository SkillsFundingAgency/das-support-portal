﻿using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using SFA.DAS.NLog.Logger;
using SFA.DAS.Support.Portal.ApplicationServices.Services;
using SFA.DAS.Support.Shared.Discovery;
using SFA.DAS.Support.Shared.SiteConnection;

namespace SFA.DAS.Support.Portal.ApplicationServices.UnitTests.Services.ManifestRepository
{
    public class WhenTestingManifestRepository
    {
        private ServiceConfiguration _siteManifests = new ServiceConfiguration();

        protected string HttpsTestsite;
        protected Mock<IFormMapper> MockFormMapper;
        protected Mock<ILog> MockLogger;
        protected Mock<ISiteConnector> MockSiteConnector;
        protected ISubSiteConnectorSettings MockSiteSettings;

        protected SiteManifest TestSiteManifest;
        protected string TestSites;
        protected Uri TestSiteUri;
        protected string TestSiteIdentifier;

        protected IManifestRepository Unit;
        protected readonly string BaseUrl = "https://testsite/";

        [SetUp]
        public virtual void Setup()
        {
            MockSiteConnector = new Mock<ISiteConnector>();
            MockFormMapper = new Mock<IFormMapper>();
            MockLogger = new Mock<ILog>();

            _siteManifests = new ServiceConfiguration();

            HttpsTestsite = $"{SupportServiceIdentity.SupportEmployerAccount}|https://testsite/";

            TestSiteManifest = new EmployerAccountSiteManifest();

            _siteManifests.Add(TestSiteManifest);

            TestSites = HttpsTestsite;

            
            MockSiteSettings = new SubSiteConnectorConfigs
            {
                SubSiteConnectorSettings = new List<SubSiteConnectorConfig>
                {
                    new SubSiteConnectorConfig
                    {
                        BaseUrl = BaseUrl,
                        Key = SupportServiceIdentity.SupportEmployerAccount.ToString(),
                        IdentifierUri = "https://doesnotexist.com/for-testing-only",
                    }
                }
            };

            var subSite = MockSiteSettings.SubSiteConnectorSettings.First();

            TestSiteUri = new Uri($"{subSite.BaseUrl}api/manifest");
            TestSiteIdentifier = subSite.IdentifierUri;

            MockSiteConnector
                .Setup(x => x.Download<SiteManifest>(TestSiteUri, TestSiteIdentifier))
                .ReturnsAsync(TestSiteManifest);
            
            Unit = new ApplicationServices.Services.ManifestRepository(
                MockSiteSettings,
                MockSiteConnector.Object,
                MockFormMapper.Object,
                MockLogger.Object, _siteManifests);
        }
    }
}