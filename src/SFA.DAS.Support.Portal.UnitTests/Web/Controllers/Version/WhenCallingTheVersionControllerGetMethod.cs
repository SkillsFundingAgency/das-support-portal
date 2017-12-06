using Moq;
using NUnit.Framework;
using SFA.DAS.Support.Portal.ApplicationServices.Services;
using SFA.DAS.Support.Portal.Web.Controllers;
using SFA.DAS.Support.Portal.Web.Models;

namespace SFA.DAS.Support.Portal.UnitTests.Web.Controllers.Version
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