using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using NUnit.Framework;

namespace SFA.DAS.Support.Portal.UnitTests.Web.Controllers.VersionController
{
    public class WhenCallingTheVersionControllerGetClaimsMethod : WithAPreparedVersionController
    {
        [Test]
        public void ItShouldReturnTheListOfUserClaims()
        {
            var actual = Unit.GetClaims();
            Assert.IsInstanceOf<IEnumerable<Claim>>(actual);
            Assert.AreEqual(0, actual.Count());
        }
    }
}