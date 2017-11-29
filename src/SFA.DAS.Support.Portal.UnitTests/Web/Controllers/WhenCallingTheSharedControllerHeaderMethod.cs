using System.Web.Mvc;
using NUnit.Framework;
using SFA.DAS.Support.Portal.Web.Controllers;
using SFA.DAS.Support.Portal.Web.ViewModels;

namespace SFA.DAS.Support.Portal.UnitTests.Web.Controllers
{
    [TestFixture]
    public class WhenCallingTheSharedControllerHeaderMethod : WhenTestingAController<SharedController>
    {

        [SetUp]
        public override void Setup()
        {
            base.Setup();
            _actionResultResponse = Unit.Header();
        }

        [Test]
        public void ItShouldReturnAPartialHeaderView()
        {
            Assert.IsInstanceOf<PartialViewResult>(_actionResultResponse);

            var expected = "_Header";
            Assert.AreEqual(expected, (_actionResultResponse as PartialViewResult).ViewName);
        }

        [Test]
        public void ItShouldReturnAUsernameInTheViewModel()
        {
            Assert.IsInstanceOf<HeaderViewModel>((_actionResultResponse as PartialViewResult).Model);
            Assert.IsNotNull(((HeaderViewModel)(_actionResultResponse as PartialViewResult).Model).Username);
        }
    }
}