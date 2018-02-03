using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Schema.Generation;
using NUnit.Framework;
using SFA.DAS.Support.Shared.Authentication;
using SFA.DAS.Support.Shared.Discovery;
using SFA.DAS.Support.Shared.SearchIndexModel;

namespace SFA.DAS.Support.Shared.Tests.Discovery
{
    [TestFixture]
    public class SupportServiceManifestsTests
    {
        private string _serviceName = "SFA.DAS.Support.ServiceManifests";

        [SetUp]
        public void Setup()
        {
            _unit = new SupportServiceManifests();
            _unit.Add(SupportServiceIdentity.SupportEmployerUser, new SiteManifest
            {
                ServiceIdentity =  SupportServiceIdentity.SupportEmployerUser,
                Resources = new[]
                {
                    new SiteResource
                    {
                        ResourceKey = SupportServiceResourceKey.EmployerUser,
                        ResourceTitle = "Overview",

                        ResourceUrlFormat = "/user/index/{0}",
                        SearchItemsUrl = "/api/manifest/search",
                        SearchCategory = SearchCategory.User
                    },
                    new SiteResource
                    {
                        ResourceKey = SupportServiceResourceKey.EmployerUserHeader,
                        ResourceUrlFormat = "/user/header/{0}"
                    },
                    new SiteResource
                    {
                        ResourceKey = SupportServiceResourceKey.EmployerUserAccountTeam,
                        ResourceUrlFormat = "/account/team/{0}",
                        ResourceTitle = "Team members"
                    }
                }
            });
            _unit.Add(SupportServiceIdentity.SupportEmployerAccount, new SiteManifest
            {
                ServiceIdentity =  SupportServiceIdentity.SupportEmployerAccount,
                Resources = new[]
                {
                    new SiteResource
                    {
                        ResourceKey = SupportServiceResourceKey.EmployerAccount,
                        ResourceUrlFormat = "/account/{0}",
                        ResourceTitle = "Organisations",
                        SearchItemsUrl = "/api/manifest/account",
                        SearchCategory = SearchCategory.Account
                    },
                    new SiteResource
                    {
                        ResourceKey = SupportServiceResourceKey.EmployerAccountHeader,
                        ResourceUrlFormat = "/account/header/{0}"
                    },
                    new SiteResource
                    {
                        ResourceKey = SupportServiceResourceKey.EmployerAccountFinance,
                        ResourceUrlFormat = "/account/finance/{0}",
                        ResourceTitle = "Finance",
                        Challenge = "account/finance"
                    }
                },
                Challenges = new List<SiteChallenge>
                {
                    new SiteChallenge
                    {
                        ChallengeKey = SupportServiceResourceKey.EmployerAccountFinance,
                        ChallengeUrlFormat = "/challenge/{0}"
                    }
                }
            });
        }

        private SupportServiceManifests _unit;
        private SupportServiceManifests _deserialisedData;
        private string _serialisedData;
        private JSchema _serialisedSchema;

        [Test]
        public void ItShouldDeserialise()
        {
            Assert.DoesNotThrow(() => _serialisedData = JsonConvert.SerializeObject(_unit, Formatting.Indented));
            Assert.DoesNotThrow(() =>
                _deserialisedData = JsonConvert.DeserializeObject<SupportServiceManifests>(_serialisedData));
            Assert.IsNotNull(_deserialisedData);
        }

        [Test]
        public void ItShouldProvideASchema()
        {
            Assert.DoesNotThrow(() =>
                _serialisedSchema = new JSchemaGenerator().Generate(typeof(SupportServiceManifests)));
            Assert.IsNotNull(_serialisedSchema);
            // TODO: Walk the properties and appply the actual values in the _unit as default values for the scehame for that property
            File.WriteAllText($@"{AppDomain.CurrentDomain.BaseDirectory}\{_serviceName}.schema.json",
                _serialisedSchema.ToString());
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