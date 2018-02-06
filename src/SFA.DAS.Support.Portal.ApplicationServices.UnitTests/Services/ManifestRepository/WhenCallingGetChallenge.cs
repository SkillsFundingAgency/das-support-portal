using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.Support.Shared.Discovery;

namespace SFA.DAS.Support.Portal.ApplicationServices.UnitTests.Services.ManifestRepository
{
    [TestFixture]
    public class WhenCallingGetChallenge : WhenTestingManifestRepository
    {
        

        [Test]
        public async Task ItShouldReturnAChallengeObject()
        {
            var result = await Unit.GetChallenge(SupportServiceResourceKey.EmployerAccountFinanceChallenge);
            Assert.IsNotNull(result);
        }

        [Test]
        public void ItShouldThrowAnExceptionWhenTheKeyIsNotFound()
        {
            Assert.ThrowsAsync<KeyNotFoundException>(() =>
                Unit.GetChallenge(SupportServiceResourceKey.EmployerUserAccountTeam));
        }
    }
}