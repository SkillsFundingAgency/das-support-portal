using NUnit.Framework;
using SFA.DAS.Support.Portal.Core.Helpers;

namespace SFA.DAS.Support.Portal.UnitTests.Web.Helpers
{
    [TestFixture]
    public class PayeSchemeObfuscatorTests
    {
        [SetUp]
        public void Setup()
        {
            _unit = new PayeSchemeObfuscator();
        }

        private PayeSchemeObfuscator _unit;

        [TestCase("1/9", "1/9")]
        [TestCase("ABC/DEFGHI", "A**/*****I")]
        [TestCase("123/456789", "1**/*****9")]
        public void ItShouldPass(string payeScheme, string expectedPayeScheme)
        {
            var actual = _unit.ObscurePayeScheme(payeScheme);
            Assert.AreEqual(expectedPayeScheme, actual);
        }
    }
}