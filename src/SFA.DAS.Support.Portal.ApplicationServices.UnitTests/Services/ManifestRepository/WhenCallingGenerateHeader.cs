using System.Net;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.Support.Shared.Discovery;

namespace SFA.DAS.Support.Portal.ApplicationServices.UnitTests.Services.ManifestRepository
{
    [TestFixture]
    public class WhenCallingGenerateHeader : WhenTestingManifestRepository
    {
       
        [Test]
        public async Task ItShouldReturnANotFoundStatusIfTheResourceDoesNotExist()
        {
            var result = await Unit.GenerateHeader(SupportServiceResourceKey.EmployerUserAccounts, "id");
            Assert.AreEqual(HttpStatusCode.NotFound, result.StatusCode);
        }

        [Test]
        public async Task ItShouldReturnANullResourceIfTheResourceDoesNotExist()
        {
            var result = await Unit.GenerateHeader(SupportServiceResourceKey.None, "id");
            Assert.IsNull(result.Resource);
        }


        [Test]
        public async Task ItShouldReturnAPageIfTheResourceDoesExistAndCanBeAccessed()
        {
            var html = "<html>This is a page</html>";

            MockSiteConnector.Setup(x => x.Download(It.IsAny<string>()))
                .ReturnsAsync(html);

            var result = await Unit.GenerateHeader(SupportServiceResourceKey.EmployerAccountFinance, "id");
            Assert.IsNotNull(result);
            Assert.IsFalse($"{result}".Contains("There was a problem downloading this asset"));
            Assert.AreEqual(html, result.Resource);
        }
    }
}