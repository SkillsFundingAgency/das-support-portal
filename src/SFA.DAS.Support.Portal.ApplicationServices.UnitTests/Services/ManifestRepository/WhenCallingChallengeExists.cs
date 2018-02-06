using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.Support.Shared.Discovery;

namespace SFA.DAS.Support.Portal.ApplicationServices.UnitTests.Services.ManifestRepository
{
    [TestFixture]
    public class WhenCallingChallengeExists : WhenTestingManifestRepository
    {
        [TearDown]
        public void Teardown()
        {
           
        }

        [Test]
        public async Task ItShouldReturnFalseIfTheChallengedoesNotExist()
        {
            var result = await Unit.ChallengeExists(SupportServiceResourceKey.EmployerAccountFinance);

            Assert.IsFalse(result);
        }

        [Test]
        public async Task ItShouldReturnTrueIfTheChallengeExists()
        {
            var result = await Unit.ChallengeExists(SupportServiceResourceKey.EmployerAccountFinanceChallenge);

            Assert.IsTrue(result);
        }
    }
}