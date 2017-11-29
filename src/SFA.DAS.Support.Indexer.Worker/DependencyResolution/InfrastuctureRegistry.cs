using System.Diagnostics;
using Microsoft.Azure.Search;
using SFA.DAS.Support.Indexer.ApplicationServices.Services;
using SFA.DAS.Support.Indexer.ApplicationServices.Settings;
using SFA.DAS.Support.Indexer.Core.Services;
using SFA.DAS.Support.Indexer.Infrastructure.AzureQueues;
using SFA.DAS.Support.Indexer.Infrastructure.AzureSearch;
using SFA.DAS.Support.Indexer.Infrastructure.Manifest;
using SFA.DAS.Support.Indexer.Infrastructure.Settings;
using StructureMap.Configuration.DSL;

namespace SFA.DAS.Support.Indexer.Worker.DependencyResolution
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