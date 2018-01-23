using System;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;

namespace SFA.DAS.Support.Portal.ApplicationServices.UnitTests.Services.ManifestRepository
{
    [TestFixture]
    public class WhenCallingGetNav : WhenTestingManifestRepository
    {
        [Test]
        public async Task ItShouldNotReturnTheNavObjectIfTheResourceTitleIsMissing()
        {
            TestSiteManifest.Resources.First(x => x.ResourceKey.StartsWith("key")).ResourceTitle = null;
            var result = await Unit.GetNav("key/header", "id");
            Assert.IsNotNull(result);
            CollectionAssert.IsEmpty(result.Items);
        }

        [Test]
        public async Task ItShouldReturnTheNavObject()
        {
            var result = await Unit.GetNav("key", "id");
            Assert.IsNotNull(result);
            CollectionAssert.IsNotEmpty(result.Items);
        }


        [Test]
        public void ItShouldThrowAnExceptionIfTheParametersAreNull()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => Unit.GetNav(null, "id"));

            Assert.ThrowsAsync<ArgumentNullException>(() => Unit.GetNav("key", null));
        }
    }
}