using System;
using System.IO;
using Newtonsoft.Json;
using NUnit.Framework;
using SFA.DAS.Support.Shared.Discovery;

namespace SFA.DAS.Support.Shared.Tests.Discovery
{

    [TestFixture]
    public class SupportServiceManifestsTests
    {
        private string _serviceName = "SFA.DAS.Support.ServiceManifests";

        [SetUp]
        public void Setup()
        {
            _unit = new ServiceConfiguration();
        }

        private ServiceConfiguration _unit;
        private ServiceConfiguration _deserialisedData;
        private string _serialisedData;
        
        [Test]
        public void ItShouldDeserialise()
        {
            Assert.DoesNotThrow(() => _serialisedData = JsonConvert.SerializeObject(_unit, Formatting.Indented));
            Assert.DoesNotThrow(() =>
                _deserialisedData = JsonConvert.DeserializeObject<ServiceConfiguration>(_serialisedData));
            Assert.IsNotNull(_deserialisedData);
        }


        [Test]
        public void ItShouldSerialise()
        {
            Assert.DoesNotThrow(() => _serialisedData = JsonConvert.SerializeObject(_unit));
            Assert.IsNotNull(_serialisedData);
            File.WriteAllText($@"{AppDomain.CurrentDomain.BaseDirectory}\{_serviceName}.json",
                _serialisedData);
        }
    }
}