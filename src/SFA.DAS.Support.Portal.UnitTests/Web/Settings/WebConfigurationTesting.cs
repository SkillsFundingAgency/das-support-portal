using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema.Generation;
using NUnit.Framework;
using SFA.DAS.Support.Common.Infrastucture.Settings;
using SFA.DAS.Support.Portal.ApplicationServices.Settings;
using SFA.DAS.Support.Portal.Web.Settings;
using SFA.DAS.Support.Shared.SiteConnection;

namespace SFA.DAS.Support.Portal.UnitTests.Web.Settings
{
    [TestFixture]
    public class WebConfigurationTesting
    {
        [SetUp]
        public void Setup()
        {
            _unit = new WebConfiguration
            {
                Authentication = new AuthSettings
                {
                    Realm = "--- configuration value goes here ---",
                    AdfsMetadata = "--- configuration value goes here ---"
                },
                Challenge = new ChallengeSettings
                {
                    ChallengeTimeoutMinutes = 3
                },
                Crypto = new CryptoSettings
                {
                    Secret = "--- configuration value goes here ---",
                    Salt = "--- configuration value goes here ---"
                },
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
                Roles = new RoleSettings
                {
                    ConsoleUserRole = "--- configuration value goes here ---",
                    ForceT2UserLocally = true,
                    GroupClaim = "--- configuration value goes here ---",
                    T2Role = "--- configuration value goes here ---",
                    Tier2Claim = "--- configuration value goes here ---"
                },
                SubSiteConnectorSettings = new List<SubSiteConnectorConfig>
                {
                    new SubSiteConnectorConfig
                    {
                        BaseUrl ="configuration",
                        IdentifierUri = "configuration",
                        Key = "configuration"
                    }
                }
            };
        }

        private const string SiteConfigFileName = "SFA.DAS.Support.Portal";

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