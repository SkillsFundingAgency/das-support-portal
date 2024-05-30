using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.Support.Portal.ApplicationServices.Models;
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
            var result = await Unit.GetResourcePage(SupportServiceResourceKey.EmployerAccountFinance, "id", "childItemId", string.Empty);
            Assert.IsFalse(string.IsNullOrWhiteSpace(result.Resource));
        }

        [Test]
        public async Task ItShouldAppendTheUriWithSupportIdWhenIncludeSupportEmailIsTrue()
        {
            const string supportEmail = "support@test.com";
            const string hashedAccountId = "FDSKJH";
            const string email = "test@email.test";
            
            var expectedUri = new Uri($"{BaseUrl}invitations/resend/{hashedAccountId}?email={WebUtility.UrlEncode(email)}&sid={WebUtility.UrlEncode(supportEmail)}");
            
            MockSiteConnector.Setup(x=> x.Download(expectedUri, MockSiteSettings.SubSiteConnectorSettings.First().IdentifierUri)).ReturnsAsync(string.Empty);
            
            await Unit.GetResourcePage(SupportServiceResourceKey.EmployerAccountInvitationConfirm, hashedAccountId, email, supportEmail);
            
            MockSiteConnector.Verify(x=> x.Download(expectedUri, MockSiteSettings.SubSiteConnectorSettings.First().IdentifierUri), Times.Once);
        }
        
        [Test]
        public async Task ItShouldNotAppendTheUriWithSupportIdWhenIncludeSupportEmailIsFalse()
        {
            const string supportEmail = "support22@test.com";
            const string hashedAccountId = "HSUEW";
            const string email = "test15@email.test";
            
            var expectedUri = new Uri($"{BaseUrl}roles/confirm/{hashedAccountId}/{WebUtility.UrlEncode(email)}");
            
            MockSiteConnector.Setup(x=> x.Download(expectedUri, MockSiteSettings.SubSiteConnectorSettings.First().IdentifierUri)).ReturnsAsync(string.Empty);
            
            await Unit.GetResourcePage(SupportServiceResourceKey.EmployerAccountChangeRoleConfirm, hashedAccountId, email, supportEmail);
            
            MockSiteConnector.Verify(x=> x.Download(expectedUri, MockSiteSettings.SubSiteConnectorSettings.First().IdentifierUri), Times.Once);
        }

        [Test]
        public void ItShouldThrowAnExceptionIfTheKeyIsNotFound()
        {
            Assert.ThrowsAsync<ManifestRepositoryException>(async () => await Unit.GetResourcePage(SupportServiceResourceKey.None, "id", "childItemId", string.Empty));
        }
    }
}