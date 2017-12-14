using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using RichardSzalay.MockHttp;
using SFA.DAS.NLog.Logger;
using SFA.DAS.Support.Portal.ApplicationServices.Services;
using SFA.DAS.Support.Portal.ApplicationServices.Settings;
using SFA.DAS.Support.Portal.Core.Services;
using SFA.DAS.Support.Portal.Infrastructure.Services;
using SFA.DAS.Support.Shared;

namespace SFA.DAS.Support.Portal.ApplicationServices.UnitTests.Services.ManifestRepository
{
    public class WhenTestingManifestRepository
    {


        protected MockHttpMessageHandler _mockHttpMessageHandler;
        protected HttpClient _httpClient;
        protected Uri _testUri = new Uri("http://localost/api/user/1234");
    

        protected IManifestRepository Unit;
        protected SiteSettings SiteSettings;
        protected ISiteConnector SiteConnector;
        protected Mock<FormMapper> MockFormMapper;
        protected Mock<ILog> MockLogger;
        protected SiteManifest SiteManifest;
        protected List<string> Sites;
        protected Mock<IProvideSettings> MockAppConfigSettingsProvider;
        protected string _httpsTestsite;
        private string _validTestResponseData;


        [SetUp]
        public void Setup()
        {
            

            _mockHttpMessageHandler = new MockHttpMessageHandler();
            _httpClient = new HttpClient(_mockHttpMessageHandler);

            
            MockAppConfigSettingsProvider = new  Mock<IProvideSettings>();
            SiteSettings = new SiteSettings( MockAppConfigSettingsProvider.Object);
            SiteConnector = new SiteConnector(_httpClient);
            MockFormMapper = new Mock<FormMapper>();
            MockLogger = new Mock<ILog>();

            Unit = new ApplicationServices.Services.ManifestRepository(
                SiteSettings, 
                SiteConnector, 
                MockFormMapper.Object, 
                MockLogger.Object);


            _httpsTestsite = "https://testsite";

            SiteManifest = new SiteManifest()
            {
                BaseUrl = _httpsTestsite,
                Challenges =  new List<SiteChallenge>()
                {
                    new SiteChallenge(){ ChallengeKey = "challengekey", ChallengeUrlFormat = "/challenge/me/{key}"}
                },
                Resources = new List<SiteResource>()
                {
                    new     SiteResource()
                    {
                        Challenge = "Tell me a secret", ResourceKey = "ResourceKey",
                        ResourceTitle = "Resource Title", ResourceUrlFormat = "/resource/{0}",
                        SearchItemsUrl = "https://testsite/search"
                    },
                    new     SiteResource()
                    {
                        Challenge = "Heads Up", ResourceKey = "key/header",
                        ResourceTitle = "Resource Title",
                        ResourceUrlFormat = "/resource/{0}",
                        SearchItemsUrl = "https://testsite/search"
                    }
                }, Version = "1.0.0.0"
            };
            Sites = new List<string>(){_httpsTestsite};



            MockAppConfigSettingsProvider.Setup(x => x.GetArray("Support:SubSite"))
                .Returns(Sites);


            _validTestResponseData = JsonConvert.SerializeObject(SiteManifest);
            


            _mockHttpMessageHandler
                .When($"{_httpsTestsite}/api/manifest")
                .Respond(HttpStatusCode.OK, "application/json", _validTestResponseData);

            
        }

        [TearDown]
        public void Teardown()
        {
            MockLogger.Verify(x=>x.Debug($"Downloading '{Sites.First()}'"),Times.Once);
        }
    }

   

    [TestFixture]
    public class WhenCallingGetChallengeForm : WhenTestingManifestRepository
    {
        [Test]
        public async Task ItShould()
        {
            var result = await Unit.GetChallengeForm("key", "id", "url");
            Assert.Fail("To Be Done");
        }
    }

    [TestFixture]
    public class WhenCallingGetChallenge : WhenTestingManifestRepository
    {
        [Test]
        public async Task ItShould()
        {
            var result = await Unit.GetChallenge("key");
            Assert.Fail("To Be Done");
        }
    }

    [TestFixture]
    public class WhenCallingGetNav : WhenTestingManifestRepository
    {
        [Test]
        public async Task ItShould()
        {
            var result = await Unit.GetNav("key", "id");
            Assert.Fail("To Be Done");
        }
    }



    [TestFixture]
    public class WhenCallingGetManifests : WhenTestingManifestRepository
    {
        [Test]
        public async Task ItShould()
        {
            var result = await Unit.GetManifests();
            Assert.Fail("To Be Done");
        }
    }


    [TestFixture]
    public class WhenCallingGetResource : WhenTestingManifestRepository
    {
        [Test]
        public async Task ItShould()
        {
            var result = await Unit.GetResource("key");
            Assert.Fail("To Be Done");
        }
    }
    [TestFixture]
    public class WhenCallingGetResourcePage : WhenTestingManifestRepository
    {
        [Test] 
        public async Task ItShould()
        {
            var result = await Unit.GetResourcePage("key","id");
            Assert.Fail("To Be Done");
        }
    }
    [TestFixture]
    public class WhenCallingSubmitChallenge : WhenTestingManifestRepository
    {
        [Test]
        public async Task ItShould()
        {
            var result = await Unit.SubmitChallenge("123", new Dictionary<string, string>());
            Assert.Fail("To Be Done");
        }
    }
}