using System.Collections.Generic;
using NUnit.Framework;
using SFA.DAS.Support.Shared.Discovery;

namespace SFA.DAS.Support.Shared.Tests.Discovery
{
    [TestFixture]
    public class WhenCallingGetChallenge 
    {
        private ServiceConfiguration _unit = new ServiceConfiguration()
        {
            new EmployerAccountSiteManifest()
        };
      

        [Test]
        public void ItShouldReturnAChallengeObject()
        {
            var result =  _unit.GetChallenge(SupportServiceResourceKey.EmployerAccountFinanceChallenge);
            Assert.IsNotNull(result);
        }

        [Test]
        public void ItShouldThrowAnExceptionWhenTheKeyIsNotFound()
        {
            Assert.IsNull(_unit.GetChallenge(SupportServiceResourceKey.EmployerAccountTeam));
        }
    }
}