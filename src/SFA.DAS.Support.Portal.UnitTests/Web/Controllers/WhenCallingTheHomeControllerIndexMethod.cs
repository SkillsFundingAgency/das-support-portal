using System.Web.Mvc;
using NUnit.Framework;
using SFA.DAS.Support.Portal.Web.Controllers;

namespace SFA.DAS.Support.Portal.UnitTests.Web.Controllers
{
    public class WhenCallingTheHomeControllerIndexMethod : WhenTestingAnMvcControllerOfType<HomeController>
    {
        [Test]
        public void ItShouldReturnAView()
        {
            ActionResultResponse = Unit.Index();
            Assert.IsInstanceOf<ViewResult>(ActionResultResponse);
        }

    }
}