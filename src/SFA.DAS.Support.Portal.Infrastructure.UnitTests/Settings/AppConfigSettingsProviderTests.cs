using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.Support.Portal.Core.Services;
using SFA.DAS.Support.Portal.Infrastructure.Settings;

namespace SFA.DAS.Support.Portal.Infrastructure.UnitTests.Settings
{
    public class AppConfigSettingsProviderTests
    {
        private IProvideSettings _unit;
        private Mock<IProvideSettings> _mockMachineSettings;
        [SetUp]
        public void Setup()
        {
            _mockMachineSettings = new Mock<IProvideSettings>();
            
            _unit = new AppConfigSettingsProvider(_mockMachineSettings.Object);
        }

        [Test]
        public void ItShouldThrowExceptionIfGetSettingIsCalledWithAnUnknownKey()
        {
            Assert.Throws<ConfigurationErrorsException>(()=> _unit.GetSetting("NonExistentSettingsKey"));
        }

        [Test]
        public void ItShouldNotThrowAnExceptionIfGetSettingIsCalledWithAKnownKey()
        {
            Assert.DoesNotThrow(() => _unit.GetSetting("KnownKey"));
        }

        [Test]
        public void ItShouldReturnTheValueWhenGetSettingIsCalledWithAKnownKey()
        {
            Assert.AreEqual( "KnownValue", _unit.GetSetting("KnownKey"));
        }



        [Test]
        public void ItShouldNotThrowAnExceptionIfGetNullableSettingIsCalledWithAnUnknownKey()
        {
            Assert.DoesNotThrow(() => _unit.GetNullableSetting("NonExistentSettingsKey"));
        }

        [Test]
        public void ItShouldNotThrowAnExceptionIfGetNullableSettingIsCalledWithAKnownKey()
        {
            Assert.DoesNotThrow(() => _unit.GetNullableSetting("KnownKey"));
        }

        [Test]
        public void ItShouldReturnTheValueWhenGetNullableSettingIsCalledWithAKnownKey()
        {
            Assert.AreEqual("KnownValue", _unit.GetNullableSetting("KnownKey"));
        }

    }
}
