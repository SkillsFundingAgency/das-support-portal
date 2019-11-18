using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema.Generation;
using NUnit.Framework;
using SFA.DAS.Support.Common.Infrastucture.Settings;
using SFA.DAS.Support.Indexer.ApplicationServices.Settings;
using SFA.DAS.Support.Indexer.Jobs;
using SFA.DAS.Support.Shared.SiteConnection;

namespace SFA.DAS.Support.Portal.Indexer.Jobs.UnitTests
{
    [TestFixture]
    public class WebConfigurationTesting
    {
        [SetUp]
        public void Setup()
        {
            _unit = new WebConfiguration
            {
                ElasticSearch = new ElasticSearchSettings
                {
                    IndexName = "--- configuration value goes here ---",
                    ServerUrls = "--- configuration value goes here ---",
                    ElasticUsername = "--- configuration value goes here ---",
                    ElasticPassword = "--- configuration value goes here ---",
                    Elk5Enabled = true,
                    IgnoreSslCertificateEnabled = true,
                    IndexShards = 1,
                    IndexReplicas = 0
                },

                Site = new SiteSettings
                {
                    BaseUrls = "https://127.0.0.1:51274,https://127.0.0.1:19722",
                    DelayTimeInSeconds = "1800"
                },
                SiteConnector = new SiteConnectorSettings
                {
                    ClientId = "--- configuration value goes here ---",
                    ClientSecret = "--- configuration value goes here ---",
                    IdentifierUri = "--- configuration value goes here ---",
                    Tenant = "--- configuration value goes here ---"
                }
            };
        }

        private const string SiteConfigFileName = "SFA.DAS.Support.Portal.Indexer.Worker";

        private WebConfiguration _unit;

        [Test]
        public void ItShouldDeserialiseFaithfuly()
        {
            var json = JsonConvert.SerializeObject(_unit);
            Assert.IsNotNull(json);
            var actual = JsonConvert.DeserializeObject<WebConfiguration>(json);
            Assert.AreEqual(json, JsonConvert.SerializeObject(actual));
        }

        [Test]
        public void ItShouldDeserialize()
        {
            var json = JsonConvert.SerializeObject(_unit);
            Assert.IsNotNull(json);
            var actual = JsonConvert.DeserializeObject<WebConfiguration>(json);
            Assert.IsNotNull(actual);
        }

        [Test]
        public void ItShouldSerialize()
        {
            var json = JsonConvert.SerializeObject(_unit);
            Assert.IsFalse(string.IsNullOrWhiteSpace(json));


            File.WriteAllText($@"{AppDomain.CurrentDomain.BaseDirectory}\{SiteConfigFileName}.json", json);
        }
    }
}