using System.Diagnostics;
using Microsoft.Azure.Search;
using SFA.DAS.Support.Indexer.ApplicationServices.Services;
using SFA.DAS.Support.Indexer.ApplicationServices.Settings;
using SFA.DAS.Support.Indexer.Core.Services;
using SFA.DAS.Support.Indexer.Infrastructure.AzureQueues;
using SFA.DAS.Support.Indexer.Infrastructure.Manifest;
using SFA.DAS.Support.Indexer.Infrastructure.Settings;
using StructureMap.Configuration.DSL;
using SFA.DAS.Support.Common.Infrastucture.Indexer;
using SFA.DAS.Support.Common.Infrastucture.Settings;
using SFA.DAS.Support.Common.Infrastucture.Elasticsearch;
using SFA.DAS.NLog.Logger;
using System.Collections.Generic;
using System.Net.Http;
using System.Reflection;
using System;

namespace SFA.DAS.Support.Indexer.Worker.DependencyResolution
{
    public class InfrastuctureRegistry : Registry
    {
        public InfrastuctureRegistry()
        {
            For<ILog>().Use(x => new NLogLogger(x.ParentType, null, GetProperties())).AlwaysUnique();

            For<HttpClient>().AlwaysUnique().Use(new HttpClient());

            For<IGetSearchItemsFromASite>().Use<ManifestProvider>();

            For<IGetSiteManifest>().Use<ManifestProvider>();

            if (Debugger.IsAttached)
                For<IProvideSettings>().Use(c => new CloudServiceSettingsProvider(new MachineSettings(string.Empty)));
            else
                For<IProvideSettings>().Use<CloudServiceSettingsProvider>();

            For<ISearchSettings>().Use<SearchSettings>();
            For<IElasticsearchClientFactory>().Use<ElasticsearchClientFactory>();
            For<IElasticsearchCustomClient>().Use<ElasticsearchCustomClient>();
            For<IIndexProvider>().Use<ElasticSearchIndexProvider>();
            For<ITrigger>().Use<StorageQueueService>();
            For<IIndexSearchItems>().Use<IndexerService>();
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