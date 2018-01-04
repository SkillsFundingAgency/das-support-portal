using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema.Generation;
using NUnit.Framework;
using SFA.DAS.Support.Common.Infrastucture.Settings;
using SFA.DAS.Support.Portal.ApplicationServices.Settings;
using SFA.DAS.Support.Portal.Infrastructure.Settings;
using SFA.DAS.Support.Portal.Web.Settings;

namespace SFA.DAS.Support.Portal.UnitTests.Web.Settings
{
    [TestFixture]
    public class WebConfigurationTesting
    {
        private const string SiteConfigFileName = "SFA.DAS.Support.Portal";
        [SetUp]
        public void Setup()
        {
            _unit = new WebConfiguration
            {
                AccountsApi = new AccountsApiConfiguration
                {
                    ApiBaseUrl = "--- configuration value goes here ---",
                    ClientId = "00000000-0000-0000-0000-000000000000",
                    ClientSecret = "--- configuration value goes here ---",
                    IdentifierUri = "--- configuration value goes here ---",
                    Tenant = "--- configuration value goes here ---"
                },
                Challenge = new ChallengeSettings
                {
                    ChallengeTimeoutMinutes = 3
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
                EmployerUsersApi = new EmployerUsersApiConfiguration()
                {
                    ApiBaseUrl = "--- configuration value goes here ---",
                    ClientId = "12312313-d123-1231-1231-123131231653",
                    ClientSecret = "--- configuration value goes here ---",
                    IdentifierUri = "--- configuration value goes here ---",
                    Tenant = "--- configuration value goes here ---",
                    ClientCertificateThumbprint = "--- configuration value goes here ---"
                },
                HmrcClient = new HmrcClientConfiguration()
                {
                    HttpClientBaseUrl = "--- configuration value goes here ---",
                },
                LevySubmissionsApi = new LevySubmissionsApiConfiguration()
                {
                    ApiBaseUrl = "--- configuration value goes here ---",
                    ClientId = "d7957772-af61-4ff1-931a-612312312310",
                    ClientSecret = "--- configuration value goes here ---",
                    IdentifierUri = "--- configuration value goes here ---",
                    Tenant = "--- configuration value goes here ---",
                    LevyTokenCertificatethumprint = "--- configuration value goes here ---"
                },
                Site = new SiteSettings()
                {
                    BaseUrls = "https://127.0.0.1:51274,https://127.0.0.1:19722"
                },
                SiteConnector = new SiteConnectorSettings()
                {
                    ApiBaseUrl = "--- configuration value goes here ---",
                    ClientId = "--- configuration value goes here ---",
                    ClientSecret = "--- configuration value goes here ---",
                    IdentifierUri = "--- configuration value goes here ---",
                    Tenant = "--- configuration value goes here ---"
                },
                Roles = new RoleSettings()
                {
                    ConsoleUserRole = "--- configuration value goes here ---",
                    ForceT2UserLocally = true,
                    GroupClaim = "--- configuration value goes here ---",
                    T2Role = "--- configuration value goes here ---",
                    Tier2Claim = "--- configuration value goes here ---"
                },
                Authentication = new AuthSettings()
                {
                    Realm = "--- configuration value goes here ---",
                    AdfsMetadata = "--- configuration value goes here ---"
                },
                Crypto = new CryptoSettings()
                {
                    Secret = "--- configuration value goes here ---",
                    Salt = "--- configuration value goes here ---"
                }

            };
        }

        private WebConfiguration _unit;

        [Test]
        public void ItShouldSerialize()
        {
            var json = JsonConvert.SerializeObject(_unit);
            Assert.IsFalse(string.IsNullOrWhiteSpace(json));


            System.IO.File.WriteAllText($@"{AppDomain.CurrentDomain.BaseDirectory}\{SiteConfigFileName}.json", json);

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
        public void ItShouldDeserialiseFaithfuly()
        {
            var json = JsonConvert.SerializeObject(_unit);
            Assert.IsNotNull(json);
            var actual = JsonConvert.DeserializeObject<WebConfiguration>(json);
            Assert.AreEqual(json, JsonConvert.SerializeObject(actual));
        }


        [Test]
        public void ItShouldGenerateASchema()
        {

            var provider = new FormatSchemaProvider();
            var jSchemaGenerator = new JSchemaGenerator();
            jSchemaGenerator.GenerationProviders.Clear();
            jSchemaGenerator.GenerationProviders.Add(provider);
            var actual = jSchemaGenerator.Generate(typeof(WebConfiguration));


            Assert.IsNotNull(actual);
            // hack to leverage format as 'environmentVariable'
            var schemaString = actual.ToString().Replace($"\"format\":", "\"environmentVariable\":");
            Assert.IsNotNull(schemaString);
            System.IO.File.WriteAllText($@"{AppDomain.CurrentDomain.BaseDirectory}\{SiteConfigFileName}.schema.json", schemaString);
        }
    }
}