using System.Web.Mvc;
using NUnit.Framework;
using SFA.DAS.Support.Portal.Web.ViewModels;

namespace SFA.DAS.Support.Portal.UnitTests.Web.Controllers.SharedController
{
    [TestFixture]
    public class WhenCallingTheSharedControllerHeaderMethod : WhenTestingAnMvcControllerOfType<Portal.Web.Controllers.SharedController>
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