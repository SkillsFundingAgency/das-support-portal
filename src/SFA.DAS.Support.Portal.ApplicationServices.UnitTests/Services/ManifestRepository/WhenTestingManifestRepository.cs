using System;
using System.Linq;
using Moq;
using NUnit.Framework;
using SFA.DAS.NLog.Logger;
using SFA.DAS.Support.Portal.ApplicationServices.Services;
using SFA.DAS.Support.Portal.ApplicationServices.Settings;
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

            _siteManifests = new ServiceConfiguration();


            HttpsTestsite = $"{SupportServiceIdentity.SupportEmployerAccount}|https://testsite/";

            TestSiteManifest = new EmployerAccountSiteManifest();

            _siteManifests.Add( TestSiteManifest);


            TestSites = HttpsTestsite;

            MockSiteSettings.SetupGet(x => x.BaseUrls)
                .Returns(TestSites);

            TestSiteUri = TestSites.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries)
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x=>x.Split(new []{'|'},StringSplitOptions.RemoveEmptyEntries ))
                .Select(x => new Uri(x[1])).First();

            TestSiteUri = new Uri($"{TestSiteUri}api/manifest");

            MockSiteConnector.Setup(x => x.Download<SiteManifest>(TestSiteUri, It.IsAny<SupportServiceResourceKey>())).ReturnsAsync(TestSiteManifest);

            Unit = new ApplicationServices.Services.ManifestRepository(
                MockSiteSettings.Object,
                MockSiteConnector.Object,
                MockFormMapper.Object,
                MockLogger.Object, _siteManifests);
        }
    }
}