using System.Diagnostics;
using SFA.DAS.Support.Indexer.ApplicationServices.Services;
using SFA.DAS.Support.Indexer.Core.Services;
using SFA.DAS.Support.Indexer.Infrastructure.AzureQueues;
using SFA.DAS.Support.Indexer.Infrastructure.Manifest;
using StructureMap.Configuration.DSL;
using SFA.DAS.Support.Common.Infrastucture.Indexer;
using SFA.DAS.Support.Common.Infrastucture.Elasticsearch;
using SFA.DAS.NLog.Logger;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Reflection;
using System;
using SFA.DAS.Support.Common.Infrastucture.Settings;
using SFA.DAS.Support.Indexer.ApplicationServices.Settings;

namespace SFA.DAS.Support.Indexer.Worker.DependencyResolution
{
    [ExcludeFromCodeCoverage]
    public class InfrastuctureRegistry : Registry
    {
        public InfrastuctureRegistry()
        {

            For<HttpClient>().AlwaysUnique().Use(new HttpClient());

            For<IGetSearchItemsFromASite>().Use<ManifestProvider>();

            For<IGetSiteManifest>().Use<ManifestProvider>();
           
            For<IElasticsearchClientFactory>().Use<ElasticsearchClientFactory>();

            For<IElasticsearchCustomClient>().Use<ElasticsearchCustomClient>();

            For<IIndexProvider>().Use<ElasticSearchIndexProvider>();

          

            For<ITrigger>().Use<StorageQueueService>();
            For<IIndexSearchItems>().Use<IndexerService>();
            For<IIndexNameCreator>().Use<IndexNameCreator>();
        }

        private IDictionary<string, object> GetProperties()
        {
            var properties = new Dictionary<string, object>();
            properties.Add("Version", GetVersion());
            return properties;
        }

        private string GetVersion()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
            return fileVersionInfo.ProductVersion;
        }



    }
}