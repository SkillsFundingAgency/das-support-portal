using NUnit.Framework;
using SFA.DAS.Support.Portal.Core.Domain.Model;

namespace SFA.DAS.Support.Portal.UnitTests.Web.Domain
{
    [TestFixture]
    public class WhenAnEmployerUserIsBothActiveAndNotLocked : WhenTestingEmployerUser
    {
        [SetUp]
        protected override void Setup()
        {
            base.Setup();
            Unit.IsActive = true;
            Unit.IsLocked = false;
        }

        [Test]
        public void ItShouldHaveAStatusOfActive()
        {
            Assert.AreEqual(UserStatus.Active, Unit.Status);
        }
    }
}