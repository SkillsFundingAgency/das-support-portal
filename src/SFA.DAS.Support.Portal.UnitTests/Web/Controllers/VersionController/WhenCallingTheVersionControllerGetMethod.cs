using NUnit.Framework;
using SFA.DAS.Support.Portal.Web.Models;

namespace SFA.DAS.Support.Portal.UnitTests.Web.Controllers.VersionController
{
    public class WhenCallingTheVersionControllerGetMethod : WithAPreparedVersionController
    {
      

        

        [Test]
        public void ItShouldReturnVersionInformation()
        {
            var actual = Unit.Get();
            Assert.IsInstanceOf<VersionInformation>(actual);   
        }

    }
}