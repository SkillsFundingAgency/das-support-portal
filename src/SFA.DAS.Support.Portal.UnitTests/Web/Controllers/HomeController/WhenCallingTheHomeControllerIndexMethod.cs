using System.Web.Mvc;
using NUnit.Framework;

namespace SFA.DAS.Support.Portal.UnitTests.Web.Controllers.HomeController
{
    public class
        WhenCallingTheHomeControllerIndexMethod : WhenTestingAnMvcControllerOfType<Portal.Web.Controllers.HomeController
        >
    {
        [Test]
        public void ItShouldReturnAView()
        {
            ActionResultResponse = Unit.Index();
            Assert.IsInstanceOf<ViewResult>(ActionResultResponse);
        }
    }
}