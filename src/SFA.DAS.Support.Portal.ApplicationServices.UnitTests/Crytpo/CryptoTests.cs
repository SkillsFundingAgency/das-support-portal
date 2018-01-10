using Moq;
using NUnit.Framework;
using SFA.DAS.Support.Portal.ApplicationServices.Services;
using SFA.DAS.Support.Portal.ApplicationServices.Settings;
using SFA.DAS.Support.Portal.Core.Services;
using SFA.DAS.Support.Portal.Infrastructure.Settings;

namespace SFA.DAS.Support.Portal.ApplicationServices.UnitTests.Crytpo
{
    [TestFixture]
    public class CryptoTests
    {
        private ICrypto _unit;
        private Mock<ICryptoSettings> _mockSettings;
     
        [SetUp]
        public void Setup()
        {
            _mockSettings = new Mock<ICryptoSettings>();

            _mockSettings.SetupGet(x => x.Salt).Returns("SALTYSWEDISHSEADOG");
            _mockSettings.SetupGet(x => x.Secret).Returns("SECRETSQUIRREL");

            _unit = new Crypto(_mockSettings.Object);

        }

        [Test]
        public void ItShouldEncryptAndDecryptSymmetrically()
        {
            string expected = "123456";
            var encryptedValue = _unit.EncryptStringAES(expected);
            var actual = _unit.DecryptStringAES(encryptedValue);
            Assert.AreEqual(expected, actual);
        }



    }
}
