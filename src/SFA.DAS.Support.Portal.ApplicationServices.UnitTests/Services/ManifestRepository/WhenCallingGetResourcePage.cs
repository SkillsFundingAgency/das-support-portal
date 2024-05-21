using System;
using System.Threading.Tasks;
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
            var result = await Unit.GetResourcePage(SupportServiceResourceKey.EmployerAccountFinance, "id", "childItemId", string.Empty);
            Assert.IsFalse(string.IsNullOrWhiteSpace(result.Resource));
        }

        [Test]
        public void ItShouldThrowAnExceptionIfTheKeyIsNotFound()
        {
            Assert.ThrowsAsync<ManifestRepositoryException>(async () => await Unit.GetResourcePage(SupportServiceResourceKey.None, "id", "childItemId", string.Empty));
        }
    }
}