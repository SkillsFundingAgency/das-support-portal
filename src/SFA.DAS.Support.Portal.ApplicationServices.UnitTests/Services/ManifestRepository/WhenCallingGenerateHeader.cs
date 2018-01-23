using System.Net;
using System.Threading.Tasks;
using System.Web;
using Moq;
using NUnit.Framework;

namespace SFA.DAS.Support.Portal.ApplicationServices.UnitTests.Services.ManifestRepository
{
    [TestFixture]
    public class WhenCallingGenerateHeader : WhenTestingManifestRepository
    {
        [TearDown]
        public void Teardown()
        {
            MockLogger.Verify(x => x.Debug($"Downloading '{TestSiteUri}'"), Times.Once);
        }

        [Test]
        public async Task ItShouldReturnANullResourceIfTheResourceDoesNotExist()
        {
            var result = await Unit.GenerateHeader("nokey", "id");
            Assert.IsNull(result.Resource);
        }
        [Test]
        public async Task ItShouldReturnANotFoundStatusIfTheResourceDoesNotExist()
        {
            var result = await Unit.GenerateHeader("nokey", "id");
            Assert.AreEqual(HttpStatusCode.NotFound,  result.StatusCode);
        }


        [Test]
        public async Task ItShouldReturnAPageIfTheResourceDoesExistAndCanBeAccessed()
        {
            var html = "<html>This is a page</html>";

            MockSiteConnector.Setup(x => x.Download(It.IsAny<string>()))
                .ReturnsAsync(html);

            var result = await Unit.GenerateHeader("key", "id");
            Assert.IsNotNull(result);
            Assert.IsFalse($"{result}".Contains("There was a problem downloading this asset"));
            Assert.AreEqual(html, result.Resource);
        }

        
    }
}