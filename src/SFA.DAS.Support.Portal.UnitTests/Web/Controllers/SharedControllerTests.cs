using System.Web.Mvc;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.Support.Portal.ApplicationServices.Queries;
using SFA.DAS.Support.Portal.Web.Controllers;
using SFA.DAS.Support.Portal.Web.Services;
using SFA.DAS.Support.Portal.Web.ViewModels;

namespace SFA.DAS.Support.Portal.UnitTests.Web.Controllers
{
    public class WhenTestingTheSharedController
    {
        protected SharedController _unit;
        protected ActionResult _actionResult;

        [SetUp]
        public virtual void Setup()
        {
            _unit = new SharedController();

        }
    }

    [TestFixture]
    public class WhenCallingTheHeaderMethod : WhenTestingTheSharedController
    {
        [SetUp]
        public override void Setup()
        {
            base.Setup();
            _actionResult = _unit.Header();
        }

        [Test]
        public void ItShouldReturnAPartialHeaderView()
        {
            Assert.IsInstanceOf<PartialViewResult>(_actionResult);

            var expected = "_Header";
            Assert.AreEqual(expected, (_actionResult as PartialViewResult).ViewName);

        }

        [Test]
        public void ItShouldReturntheCurrentUsernameInTheViewModel()
        {



            Assert.IsInstanceOf<HeaderViewModel>((_actionResult as PartialViewResult).Model);

            Assert.IsNotNull(((HeaderViewModel)(_actionResult as PartialViewResult).Model).Username);




        }
    }
}
