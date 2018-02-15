using System.Reflection;
using NUnit.Framework;

namespace SFA.DAS.Support.Shared.Tests.Extensions
{
    [TestFixture]
    public class ExtensionsTests
    {
        [Test]
        public void ItshouldRevealThisAssemblyInfoDescription()
        {
            Assert.AreEqual("Shared code and functions common to all support sub systems",
                Assembly.GetExecutingAssembly().Description());
        }
        [Test]
        public void ItshouldRevealhisAssemblyInfoFileVersion()
        {
            Assert.AreEqual("1.0.0.0", Assembly.GetExecutingAssembly().FileVersion());
        } [Test]
        public void ItshouldRevealhisAssemblyInfoId()
        {
            Assert.AreEqual("db114c77-4906-4f20-9824-c01beeb26edc", Assembly.GetExecutingAssembly().Id());
        }
        [Test]
        public void ItshouldRevealhisAssemblyInfoProduct()
        {
            Assert.AreEqual("SFA.DAS.Support.Shared.Tests", Assembly.GetExecutingAssembly().Product());
        }

        [Test]
        public void ItshouldRevealhisAssemblyInfoVersion()
        {
            Assert.AreEqual("", Assembly.GetExecutingAssembly().Version());
        }

       
        [Test]
        public void ItshouldRevealThisAssemblyInfoCulture()
        {
            Assert.AreEqual("", Assembly.GetExecutingAssembly().Culture());
        }

        [Test]
        public void ItshouldRevealThisAssemblyInfoConfiguration()
        {
            Assert.AreEqual("", Assembly.GetExecutingAssembly().Configuration());
        }

        [Test]
        public void ItshouldRevealThisAssemblyInfo()
        {
            Assert.AreEqual("", Assembly.GetExecutingAssembly().Version());
        }

        [Test]
        public void ItshouldRevealThisAssemblyInfoTrademark()
        {
            Assert.AreEqual("", Assembly.GetExecutingAssembly().Trademark());
        }


        [Test]
        public void ItshouldRevealThisAssemblyInfoCompany()
        {
            Assert.AreEqual("Education & Skills Funding Agency", Assembly.GetExecutingAssembly().Company());
        }

        [Test]
        public void ItshouldRevealThisAssemblyInfoCopyright()
        {
            Assert.AreEqual("Copyright © 2018 Education & Skills Funding Agency", Assembly.GetExecutingAssembly().Copyright());
        }

        [Test]
        public void ItShouldRevealThisAssemblyInfoTitle()
        {
            Assert.AreEqual("SFA.DAS.Support.Shared.Tests", Assembly.GetExecutingAssembly().Title());
        }
    }
}