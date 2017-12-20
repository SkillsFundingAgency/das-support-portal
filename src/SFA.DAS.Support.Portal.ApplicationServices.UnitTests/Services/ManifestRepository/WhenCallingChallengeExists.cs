using System.Linq;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;

namespace SFA.DAS.Support.Portal.ApplicationServices.UnitTests.Services.ManifestRepository
{
    [TestFixture]
    public class WhenCallingChallengeExists : WhenTestingManifestRepository
    {
        [TearDown]
        public void Teardown()
        {
            MockLogger.Verify(x => x.Debug($"Downloading '{TestSites.First()}'"), Times.Once);
        }

        [Test]
        public async Task ItShouldReturnFalseIfTheChallengedoesNotExist()
        {
            var result = await Unit.ChallengeExists("key");

            Assert.IsFalse(result);
        }

        [Test]
        public async Task ItShouldReturnTrueIfTheChallengeExists()
        {
            var result = await Unit.ChallengeExists("ChallengeKey");

            Assert.IsTrue(result);
        }
    }
}