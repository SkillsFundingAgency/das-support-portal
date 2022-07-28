using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Moq.Protected;
using NUnit.Framework;
using Microsoft.Azure.Services.AppAuthentication;
using SFA.DAS.Support.Shared.SiteConnection;

namespace SFA.DAS.Support.Shared.Tests.SiteConnector
{
    [TestFixture]
    public class UnAuthorisedSiteConnectorTests : SiteConnectorTestBase
    {
        [Test]
        public async Task GivenClientAuthIsEmptyItShouldRequestATokenForUnAuthorisedClientsUsingMI()
        {
            MockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync((HttpRequestMessage request, CancellationToken token) => new HttpResponseMessage(System.Net.HttpStatusCode.OK) { Content = new StringContent(ValidTestResponseData) });

            var azureServiceTokenProvider = new Mock<AzureServiceTokenProvider>();

            HttpClient.DefaultRequestHeaders.Authorization = null;

            Unit = new SiteConnection.SiteConnector(HttpClient,
              MockClientAuthenticator.Object,
              Handlers,
              MockLogger.Object,
              MockazureClientCredentialHelper.Object);

            var response = await Unit.Download(new Uri(TestUrl), TestResourceIdentifier);

            Assert.IsNotNull(response);
            Assert.IsInstanceOf<string>(response);
            Assert.IsNotNull(HttpClient.DefaultRequestHeaders.Authorization);

            MockazureClientCredentialHelper
              .Verify(x => x.GetAccessTokenAsync(It.IsAny<string>()), Times.AtLeastOnce);
        }

        [Test]
        public async Task GivenClientAuthIsSuppliedItShouldRequestATokenForUnAuthorisedClientsUsingAD()
        {
            MockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync((HttpRequestMessage request, CancellationToken token) => new HttpResponseMessage(System.Net.HttpStatusCode.OK) { Content = new StringContent(ValidTestResponseData) });

            var azureServiceTokenProvider = new Mock<AzureServiceTokenProvider>();

            HttpClient.DefaultRequestHeaders.Authorization = null;

            Unit = new SiteConnection.SiteConnector(HttpClient,
              MockClientAuthenticator.Object,
              Handlers,
              MockLogger.Object,
              MockazureClientCredentialHelper.Object);

            var response = await Unit.Download(new Uri(TestUrl), TestResourceIdentifier);

            Assert.IsNotNull(response);
            Assert.IsInstanceOf<string>(response);
            Assert.IsNotNull(HttpClient.DefaultRequestHeaders.Authorization);

            MockazureClientCredentialHelper
              .Verify(x => x.GetAccessTokenAsync(It.IsAny<string>()), Times.Never);

            MockClientAuthenticator
                .Verify(x => x.Authenticate(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }
    }
}