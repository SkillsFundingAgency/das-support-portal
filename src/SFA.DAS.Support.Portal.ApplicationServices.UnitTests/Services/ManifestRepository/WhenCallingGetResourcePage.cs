using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Moq;
using NUnit.Framework;
using SFA.DAS.Support.Portal.ApplicationServices.Services;
using SFA.DAS.Support.Shared.Discovery;

namespace SFA.DAS.Support.Portal.ApplicationServices.UnitTests.Services.ManifestRepository
{
    [TestFixture]
    public class WhenCallingGetResourcePage : WhenTestingManifestRepository
    {
        [Test]
        public async Task ItShouldReturnTheHtmlPage()
        {
            const string html = "<html>Some page</html>";
            MockSiteConnector.Setup(x => x.Download(It.IsAny<Uri>(), It.IsAny<string>()))
                .ReturnsAsync(html);
            var result = await Unit.GetResourcePage(SupportServiceResourceKey.EmployerAccountFinance, "id", "childItemId");
            Assert.IsFalse(string.IsNullOrWhiteSpace(result.Resource));
        }

        [Test]
        public async Task ItShouldCallDownloadOnTheSiteConnectorWithTheCorrectUrl()
        {
            const string hashedAccountId = "HSUEW";
            const string email = "test15@email.test";

            var expectedUri = new Uri($"{BaseUrl}roles/confirm/{hashedAccountId}/{Uri.EscapeDataString(email)}");

            MockSiteConnector.Setup(x => x.Download(expectedUri, MockSiteSettings.SubSiteConnectorSettings.First().IdentifierUri)).ReturnsAsync(string.Empty);

            await Unit.GetResourcePage(SupportServiceResourceKey.EmployerAccountChangeRoleConfirm, hashedAccountId, email);

            MockSiteConnector.Verify(x => x.Download(expectedUri, MockSiteSettings.SubSiteConnectorSettings.First().IdentifierUri), Times.Once);
        }
        
        [Test]
        public async Task ItShouldCallDownloadOnTheSiteConnectorWithTheCorrectUrlWhenThereIsSpecialCharacterInChildId()
        {
            const string hashedAccountId = "SHEMDAS";
            const string email = "test+some#thing@email.test";

            var expectedUri = new Uri($"{BaseUrl}roles/confirm/{hashedAccountId}/{Uri.EscapeDataString(email)}");

            MockSiteConnector.Setup(x => x.Download(expectedUri, MockSiteSettings.SubSiteConnectorSettings.First().IdentifierUri)).ReturnsAsync(string.Empty);

            await Unit.GetResourcePage(SupportServiceResourceKey.EmployerAccountChangeRoleConfirm, hashedAccountId, email);

            MockSiteConnector.Verify(x => x.Download(expectedUri, MockSiteSettings.SubSiteConnectorSettings.First().IdentifierUri), Times.Once);
        }
        
        [Test]
        public async Task ItShouldCallDownloadOnTheSiteConnectorWithTheCorrectUrlWhenChildIdIsNull()
        {
            const string hashedAccountId = "SHEMDAS";

            var expectedUri = new Uri($"{BaseUrl}roles/confirm/{hashedAccountId}/");

            MockSiteConnector.Setup(x => x.Download(expectedUri, MockSiteSettings.SubSiteConnectorSettings.First().IdentifierUri)).ReturnsAsync(string.Empty);

            await Unit.GetResourcePage(SupportServiceResourceKey.EmployerAccountChangeRoleConfirm, hashedAccountId, null);

            MockSiteConnector.Verify(x => x.Download(expectedUri, MockSiteSettings.SubSiteConnectorSettings.First().IdentifierUri), Times.Once);
        }

        [Test]
        public void ItShouldThrowAnExceptionIfTheKeyIsNotFound()
        {
            Assert.ThrowsAsync<ManifestRepositoryException>(async () => await Unit.GetResourcePage(SupportServiceResourceKey.None, "id", "childItemId"));
        }
    }
}