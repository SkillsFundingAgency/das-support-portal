using System.Net;
using System.Threading.Tasks;
using NUnit.Framework;
using RichardSzalay.MockHttp;

namespace SFA.DAS.Support.Portal.ApplicationServices.UnitTests.Services.ManifestRepository
{
    [TestFixture]
    public class WhenCallingGenerateHeader : WhenTestingManifestRepository
    {

        [Test]
        public async Task ItShouldReturnAnEmtpyStringIfTheResourceDoesNotExist()
        {
            var result = await Unit.GenerateHeader("nokey", "id");
            Assert.AreEqual(string.Empty, result);
        }

        [Test]
        public async Task ItShouldReturnAPageIfTheResourceDoesExistAndCanBeAccessed()
        {
            var html = "<html>This is a page</html>";   
            _mockHttpMessageHandler
                .When($"{_httpsTestsite}:443/resource/id?parent=")
                .Respond(HttpStatusCode.OK, "application/json", html);

            var result = await Unit.GenerateHeader("key", "id");
            Assert.IsNotNull(result);
            Assert.IsFalse($"{result}".Contains("There was a problem downloading this asset"));
            Assert.AreEqual(html, result);
        }

        [Test]
        public async Task ItShouldReturnAProblemDownloadingIfTheResourceDoesExistButCannotBeAccessed()
        {
            var result = await Unit.GenerateHeader("key", "id");
            Assert.IsTrue($"{result}".Contains("There was a problem downloading this asset"));
        }
    }
}