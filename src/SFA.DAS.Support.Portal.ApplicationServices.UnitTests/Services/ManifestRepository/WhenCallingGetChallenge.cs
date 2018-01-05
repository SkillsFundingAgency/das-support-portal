using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;

namespace SFA.DAS.Support.Portal.ApplicationServices.UnitTests.Services.ManifestRepository
{
    [TestFixture]
    public class WhenCallingGetChallenge : WhenTestingManifestRepository
    {
        [TearDown]
        public void Teardown()
        {
            MockLogger.Verify(x => x.Debug($"Downloading '{TestSiteUri}'"), Times.Once);
        }

        [Test]
        public async Task ItShouldReturnAChallengeObject()
        {
            var result = await Unit.GetChallenge("challengekey");
            Assert.IsNotNull(result);
        }

        [Test]
        public void ItShouldThrowAnExceptionWhenTheKeyIsNotFound()
        {
            Assert.ThrowsAsync<KeyNotFoundException>(() => Unit.GetChallenge("key"));
        }
    }
}