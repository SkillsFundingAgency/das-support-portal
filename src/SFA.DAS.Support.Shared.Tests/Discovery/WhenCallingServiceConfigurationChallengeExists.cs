using NUnit.Framework;
using SFA.DAS.Support.Shared.Discovery;

namespace SFA.DAS.Support.Shared.Tests.Discovery
{
   

    public class WhenCallingServiceConfigurationChallengeExists
    {
        private ServiceConfiguration _unit = new ServiceConfiguration()
        {
            new EmployerAccountSiteManifest()
        };
      

        [Test]
        public void  ItShouldReturnFalseIfTheChallengeDoesNotExist()
        {
            var result = _unit.ChallengeExists(SupportServiceResourceKey.None);

            Assert.IsFalse(result);
        }

        [Test]
        public void ItShouldReturnTrueIfTheChallengeExists()
        {
            var result =  _unit.ChallengeExists(SupportServiceResourceKey.EmployerAccountFinanceChallenge);

            Assert.IsTrue(result);
        }
    }

}