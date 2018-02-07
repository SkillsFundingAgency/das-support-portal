using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
            foreach (var siteResource in TestSiteManifest.Resources.Where(x=>x.IsNavigationItem))
            {
                siteResource.IsNavigationItem = false;
            }
            
            var result = await Unit.GetNav(TestSiteManifest.Resources.First().ResourceKey, "id");
            Assert.IsNotNull(result);
            CollectionAssert.IsEmpty(result.Items);
        }

        [Test]
        public async Task ItShouldReturnTheNavObject()
        {
            var result = await Unit.GetNav(TestSiteManifest.Resources.First(x=>x.ResourceTitle != null).ResourceKey, "id");
            Assert.IsNotNull(result);
            CollectionAssert.IsNotEmpty(result.Items);
        }


        [Test]
        public void ItShouldThrowAnExceptionIfTheIdParameterIsNull()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() =>
                Unit.GetNav(TestSiteManifest.Resources.First(x=>x.ResourceTitle != null).ResourceKey, null));
        }
    }
}