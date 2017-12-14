using System.Threading.Tasks;
using NUnit.Framework;

namespace SFA.DAS.Support.Portal.ApplicationServices.UnitTests.Services.ManifestRepository
{
    [TestFixture]
    public class WhenCallingChallengeExists : WhenTestingManifestRepository
    {
        [Test]
        public async Task ItShouldReturnTrueIfTheChallengeExists()
        {
            var result = await Unit.ChallengeExists("ChallengeKey");
            Assert.IsTrue(result);
        }

        [Test]
        public async Task ItShouldReturnFalseIfTheChallengedoesNotExist()
        {
            var result = await Unit.ChallengeExists("key");
            Assert.IsFalse(result);
        }
    }
}