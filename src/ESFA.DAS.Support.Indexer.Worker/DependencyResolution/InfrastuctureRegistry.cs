using System.Diagnostics;
using ESFA.DAS.Support.Indexer.ApplicationServices.Services;
using ESFA.DAS.Support.Indexer.ApplicationServices.Settings;
using ESFA.DAS.Support.Indexer.Core.Services;
using ESFA.DAS.Support.Indexer.Infrastructure.AzureQueues;
using ESFA.DAS.Support.Indexer.Infrastructure.AzureSearch;
using ESFA.DAS.Support.Indexer.Infrastructure.Manifest;
using ESFA.DAS.Support.Indexer.Infrastructure.Settings;
using Microsoft.Azure.Search;
using StructureMap.Configuration.DSL;

namespace ESFA.DAS.Support.Indexer.Worker.DependencyResolution
{
    public class InfrastuctureRegistry : Registry
    {
        public InfrastuctureRegistry()
        {
            For<IGetSearchItemsFromASite>().Use<ManifestProvider>();
            For<IGetSiteManifest>().Use<ManifestProvider>();
            For<IIndexProvider>().Use<AzureSearchProvider>();
            if (Debugger.IsAttached)
                For<IProvideSettings>().Use(c => new CloudServiceSettingsProvider(new MachineSettings(string.Empty)));
            else
                For<IProvideSettings>().Use<CloudServiceSettingsProvider>();

            For<ITrigger>().Use<StorageQueueService>();
            For<IIndexSearchItems>().Use<IndexerService>();
            For<ISearchServiceClient>().Use("", x =>
            {
                var settings = x.GetInstance<ISearchSettings>();
                return new SearchServiceClient(settings.ServiceName, new SearchCredentials(settings.AdminApiKey));
            });
        }
    }
}