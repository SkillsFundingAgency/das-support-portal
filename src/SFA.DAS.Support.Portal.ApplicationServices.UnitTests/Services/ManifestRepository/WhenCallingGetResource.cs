using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;
using SFA.DAS.Support.Shared.Discovery;

namespace SFA.DAS.Support.Portal.ApplicationServices.UnitTests.Services.ManifestRepository
{
    [TestFixture]
    public class WhenCallingGetResource : WhenTestingManifestRepository
    {
        [Test]
        public async Task ItShouldReturnTheResourceObject()
        {
            var result = await Unit.GetResource(SupportServiceResourceKey.EmployerAccountFinance);
            Assert.IsNotNull(result);
        }

        [Test]
        public void ItShouldThrowAnExceptionIfTheKeyIsNotFound()
        {
            Assert.ThrowsAsync<KeyNotFoundException>(() =>
                Unit.GetResource(SupportServiceResourceKey.EmployerUserAccountTeam));
        }
    }
}