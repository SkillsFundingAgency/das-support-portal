using NUnit.Framework;
using SFA.DAS.Support.Portal.Core.Domain.Model;

namespace SFA.DAS.Support.Portal.UnitTests.Web.Domain
{
    [TestFixture]
    public class WhenAnEmployerUserIsBothActiveAndLocked : WhenTestingEmployerUser
    {
        [SetUp]
        protected override void Setup()
        {
            base.Setup();
            Unit.IsActive = true;
            Unit.IsLocked = true;
        }

        [Test]
        public void ItShouldHaveAStatusOfLocked()
        {
            Assert.AreEqual(UserStatus.Locked, Unit.Status);
        }
    }
}