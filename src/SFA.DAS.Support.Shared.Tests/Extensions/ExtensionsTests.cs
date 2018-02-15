using System.Reflection;
using NUnit.Framework;

namespace SFA.DAS.Support.Shared.Tests.Extensions
{
    [TestFixture]
    public class ExtensionsTests
    {
        [Test]
        public void ItShouldRevealThisAssemblyInfoCompany()
        {
            Assert.AreEqual("Education & Skills Funding Agency", Assembly.GetExecutingAssembly().Company());
        }

        [Test]
        public void ItShouldRevealThisAssemblyInfoConfiguration()
        {
            Assert.AreEqual("", Assembly.GetExecutingAssembly().Configuration());
        }

        [Test]
        public void ItShouldRevealThisAssemblyInfoCopyright()
        {
            Assert.AreEqual("Copyright © 2018 Education & Skills Funding Agency",
                Assembly.GetExecutingAssembly().Copyright());
        }


        [Test]
        public void ItShouldRevealThisAssemblyInfoCulture()
        {
            Assert.AreEqual("", Assembly.GetExecutingAssembly().Culture());
        }

        [Test]
        public void ItShouldRevealThisAssemblyInfoDescription()
        {
            Assert.AreEqual("Shared code and functions common to all support sub systems",
                Assembly.GetExecutingAssembly().Description());
        }

        [Test]
        public void ItShouldRevealThisAssemblyInfoFileVersion()
        {
            Assert.AreEqual("1.0.0.0", Assembly.GetExecutingAssembly().FileVersion());
        }

        [Test]
        public void ItShouldRevealThisAssemblyInfoId()
        {
            Assert.AreEqual("db114c77-4906-4f20-9824-c01beeb26edc", Assembly.GetExecutingAssembly().Id());
        }

        [Test]
        public void ItShouldRevealThisAssemblyInfoProduct()
        {
            Assert.AreEqual("SFA.DAS.Support.Shared.Tests", Assembly.GetExecutingAssembly().Product());
        }

        [Test]
        public void ItShouldRevealThisAssemblyInfoTitle()
        {
            Assert.AreEqual("SFA.DAS.Support.Shared.Tests", Assembly.GetExecutingAssembly().Title());
        }


        [Test]
        public void ItShouldRevealThisAssemblyInfoTrademark()
        {
            Assert.AreEqual("", Assembly.GetExecutingAssembly().Trademark());
        }

        [Test]
        public void ItShouldRevealThisAssemblyInfoVersion()
        {
            Assert.AreEqual("1.0.0.0", Assembly.GetExecutingAssembly().Version());
        }
    }
}