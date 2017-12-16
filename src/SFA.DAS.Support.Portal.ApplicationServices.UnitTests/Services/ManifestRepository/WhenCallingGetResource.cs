using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;

namespace SFA.DAS.Support.Portal.ApplicationServices.UnitTests.Services.ManifestRepository
{
    [TestFixture]
    public class WhenCallingGetResource : WhenTestingManifestRepository
    {
        [Test]
        public async Task ItShouldReturnTheResourceObject()
        {
            var result = await Unit.GetResource("ResourceKey".ToLower());
            Assert.IsNotNull(result);
        }

        [Test]
        public void ItShouldThrowAnExceptionIfTheKeyIsNotFound()
        {
            Assert.ThrowsAsync<KeyNotFoundException>(() => Unit.GetResource("keywhichisnotfound"));
        }
    }
}