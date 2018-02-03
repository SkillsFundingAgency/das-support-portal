using System;
using System.Threading.Tasks;
using System.Web;
using Moq;
using NUnit.Framework;
using SFA.DAS.Support.Shared.Discovery;

namespace SFA.DAS.Support.Portal.ApplicationServices.UnitTests.Services.ManifestRepository
{
    [TestFixture]
    public class WhenCallingGetManifests : WhenTestingManifestRepository
    {
       
        [Test]
        public async Task ItShouldReturnTheListOfManifestObjects()
        {
            var result = await Unit.GetManifests();

            CollectionAssert.IsNotEmpty(result);
        }
    }
}