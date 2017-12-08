using System;
using NUnit.Framework;
using SFA.DAS.Support.Portal.Core.Domain.Model;

namespace SFA.DAS.Support.Portal.UnitTests.Web.Domain
{
    [TestFixture]
    public class WhenAnEmployerUserIsBothActiveAndLocked: WhenTestingEmployerUser
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
            Assert.AreEqual(UserStatus.Locked , Unit.Status);
        }

    }
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
    public class WhenTestingEmployerUser
    {
        protected EmployerUser Unit;

        [SetUp]
        protected virtual void Setup()
        {
            Unit = Activator.CreateInstance<EmployerUser>();
        }
    }
}