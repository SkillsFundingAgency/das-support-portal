using System;
using System.Net;
using System.Threading.Tasks;
using NUnit.Framework;
using RichardSzalay.MockHttp;

namespace SFA.DAS.Support.Shared.Tests.SiteConnector
{
    [TestFixture]
    public class UnAuthorisedSiteConnectorTests : SiteConnectorTestBase
    {
        [Test]
        public async Task ItShouldRequestATokenForUnAuthorisedClients()
        {
            var code = HttpStatusCode.OK;
            MockHttpMessageHandler
                .When(TestUrl)
                .Respond(code, "application/json", ValidTestResponseData);

            HttpClient.DefaultRequestHeaders.Authorization = null;
            var response = await Unit.Download(new Uri(TestUrl));

            Assert.IsNotNull(response);
            Assert.IsInstanceOf<string>(response);
            Assert.IsNotNull(HttpClient.DefaultRequestHeaders.Authorization);
        }
    }
}