using System;
using Moq;
using NUnit.Framework;
using SFA.DAS.Support.Portal.ApplicationServices.Services;
using SFA.DAS.Support.Portal.ApplicationServices.Settings;

namespace SFA.DAS.Support.Portal.ApplicationServices.UnitTests.Crytpo
{
    [TestFixture]
    public class CryptoFailureTests
    {
        [SetUp]
        public void Setup()
        {
            _mockSettings = new Mock<ICryptoSettings>();

            _mockSettings.SetupGet(x => x.Salt).Returns("SALTYSWEDISHSEADOG");
            _mockSettings.SetupGet(x => x.Secret).Returns(null as string);

            _unit = new Crypto(_mockSettings.Object);
        }

        private ICrypto _unit;
        private Mock<ICryptoSettings> _mockSettings;

        [Test]
        public void ItShouldThrowAnDecryptionOnEncryptionWithANullSecret()
        {
            Assert.Throws<ArgumentNullException>(() => _unit.DecryptStringAES("13123123"));
        }

        [Test]
        public void ItShouldThrowAnExceptionIfTheTextIsEmpty()
        {
            Assert.Throws<ArgumentNullException>(() => _unit.EncryptStringAES(string.Empty));
        }

        [Test]
        public void ItShouldThrowAnExceptionIfTheTextIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => _unit.EncryptStringAES(null));
        }

        [Test]
        public void ItShouldThrowAnExceptionIfWithDecryptTheTextIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => _unit.DecryptStringAES(null));
        }


        [Test]
        public void ItShouldThrowAnExceptionOnEncryptionWithANullSecret()
        {
            Assert.Throws<ArgumentNullException>(() => _unit.EncryptStringAES("13123123"));
        }


        [Test]
        public void ItShouldThrowAnExceptionWithDecryptIfTheTextIsEmpty()
        {
            Assert.Throws<ArgumentNullException>(() => _unit.DecryptStringAES(string.Empty));
        }
    }
}