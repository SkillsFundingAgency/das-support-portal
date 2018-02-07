using System.Collections.Generic;
using NUnit.Framework;
using SFA.DAS.Support.Shared.Discovery;

namespace SFA.DAS.Support.Shared.Tests.Discovery
{
    [TestFixture]
    public class WhenCallingGetResource 
    {
        private ServiceConfiguration _unit = new ServiceConfiguration()
        {
            new EmployerAccountSiteManifest()
        };
      
        [Test]
        public void ItShouldReturnTheResourceObject()
        {
            var result = _unit.GetResource(SupportServiceResourceKey.EmployerAccountFinance);
            Assert.IsNotNull(result);
        }

        [Test]
        public void ItShouldThrowAnExceptionIfTheKeyIsNotFound()
        {
            Assert.IsNull(_unit.GetResource(SupportServiceResourceKey.None));
        }
    }
}