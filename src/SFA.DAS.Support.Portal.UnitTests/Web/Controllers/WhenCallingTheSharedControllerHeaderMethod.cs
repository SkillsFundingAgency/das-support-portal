using System.Web.Mvc;
using NUnit.Framework;
using SFA.DAS.Support.Portal.Web.Controllers;
using SFA.DAS.Support.Portal.Web.ViewModels;

namespace SFA.DAS.Support.Portal.UnitTests.Web.Controllers
{
    [TestFixture]
    public class WhenCallingTheSharedControllerHeaderMethod : WhenTestingAnMvcControllerOfType<SharedController>
    {

        [SetUp]
        public override void Setup()
        {
            base.Setup();
            ActionResultResponse = Unit.Header();
        }

        [Test]
        public void ItShouldReturnAPartialHeaderView()
        {
            Assert.IsInstanceOf<PartialViewResult>(ActionResultResponse);

            var expected = "_Header";
            Assert.AreEqual(expected, (ActionResultResponse as PartialViewResult).ViewName);
        }

        [Test]
        public void ItShouldReturnAUsernameInTheViewModel()
        {
            Assert.IsInstanceOf<HeaderViewModel>((ActionResultResponse as PartialViewResult).Model);
            Assert.IsNotNull(((HeaderViewModel)(ActionResultResponse as PartialViewResult).Model).Username);
        }
    }
}