using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Moq.Protected;
using NUnit.Framework;

namespace SFA.DAS.Support.Shared.Tests.SiteConnector
{
    [TestFixture]
    public class UnAuthorisedSiteConnectorTests : SiteConnectorTestBase
    {
        [Test]
        public async Task ItShouldRequestATokenForUnAuthorisedClients()
        {
            MockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync((HttpRequestMessage request, CancellationToken token) => new HttpResponseMessage(System.Net.HttpStatusCode.OK) { Content = new StringContent(ValidTestResponseData) });

            HttpClient.DefaultRequestHeaders.Authorization = null;
            var response = await Unit.Download(new Uri(TestUrl));

            Assert.IsNotNull(response);
            Assert.IsInstanceOf<string>(response);
            Assert.IsNotNull(HttpClient.DefaultRequestHeaders.Authorization);
        }
    }
}