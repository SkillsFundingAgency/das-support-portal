using NUnit.Framework;
using SFA.DAS.Support.Portal.Core.Domain.Model;

namespace SFA.DAS.Support.Portal.UnitTests.Web.Domain
{
    [TestFixture]
    public class WhenAnEmployerUserIsNeitherActiveNorLocked : WhenTestingEmployerUser
    {
        [SetUp]
        protected override void Setup()
        {
            base.Setup();
            Unit.IsActive = false;
            Unit.IsLocked = false;
        }

        [Test]
        public void ItShouldHaveAStatusOfUnverified()
        {
            Assert.AreEqual(UserStatus.Unverified, Unit.Status);
        }

    }
}