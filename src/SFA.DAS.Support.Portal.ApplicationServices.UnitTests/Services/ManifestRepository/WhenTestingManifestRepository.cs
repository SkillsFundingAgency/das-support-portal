﻿using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using SFA.DAS.NLog.Logger;
using SFA.DAS.Support.Portal.ApplicationServices.Services;
using SFA.DAS.Support.Portal.ApplicationServices.Settings;
using SFA.DAS.Support.Shared;

namespace SFA.DAS.Support.Portal.ApplicationServices.UnitTests.Services.ManifestRepository
{
    public class WhenTestingManifestRepository
    {
        protected string HttpsTestsite;
        protected Mock<IFormMapper> MockFormMapper;
        protected Mock<ILog> MockLogger;
        protected Mock<ISiteConnector> MockSiteConnector;
        protected Mock<ISiteSettings> MockSiteSettings;

        protected SiteManifest TestSiteManifest;
        protected string TestSites;
       
        protected IManifestRepository Unit;
        protected Uri TestSiteUri;


        [SetUp]
        public virtual void Setup()
        {
            MockSiteSettings = new Mock<ISiteSettings>();
            MockSiteConnector = new Mock<ISiteConnector>();
            MockFormMapper = new Mock<IFormMapper>();
            MockLogger = new Mock<ILog>();

            Unit = new ApplicationServices.Services.ManifestRepository(
                MockSiteSettings.Object,
                MockSiteConnector.Object,
                MockFormMapper.Object,
                MockLogger.Object);


            HttpsTestsite = "https://testsite";

            TestSiteManifest = new SiteManifest
            {
                BaseUrl = HttpsTestsite,
                Challenges = new List<SiteChallenge>
                {
                    new SiteChallenge
                    {
                        ChallengeKey = "challengekey",
                        ChallengeUrlFormat = "/challenge/me/{0}"
                    }
                },
                Resources = new List<SiteResource>
                {
                    new SiteResource
                    {
                        Challenge = "Tell me a secret",
                        ResourceKey = "resourcekey",
                        ResourceTitle = "Resource Title",
                        ResourceUrlFormat = "/resource/{0}",
                        SearchItemsUrl = "https://testsite/search"
                    },
                    new SiteResource
                    {
                        Challenge = "Heads Up",
                        ResourceKey = "key/header",
                        ResourceTitle = "Resource Title",
                        ResourceUrlFormat = "/resource/{0}",
                        SearchItemsUrl = "https://testsite/search"
                    }
                },
                Version = "1.0.0.0"
            };

            TestSites = HttpsTestsite;

            MockSiteSettings.SetupGet(x => x.BaseUrls)
                .Returns(TestSites);

            TestSiteUri = TestSites.Split(new []{','}, StringSplitOptions.RemoveEmptyEntries).Where(x=>!string.IsNullOrWhiteSpace(x)).Select(x=> new Uri(x)).First();
            TestSiteUri = new Uri($"{TestSiteUri}api/manifest");

            MockSiteConnector.Setup(x => x.Download<SiteManifest>(TestSiteUri)).ReturnsAsync(TestSiteManifest);
        }
    }
}