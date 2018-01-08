﻿using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema.Generation;
using NUnit.Framework;
using SFA.DAS.Support.Common.Infrastucture.Settings;
using SFA.DAS.Support.Indexer.ApplicationServices.Settings;
using SFA.DAS.Support.Indexer.Worker;

namespace SFA.DAS.Support.Portal.Indexer.Worker.UnitTests
{
    [TestFixture]
    public class WebConfigurationTesting
    {
        private const string  SiteConfigFileName = "SFA.DAS.Support.Portal.Indexer.Worker";
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
               
                Site = new SiteSettings()
                {
                    BaseUrls ="https://127.0.0.1:51274,https://127.0.0.1:19722", 
                    EnvironmentName = "LOCAL"
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