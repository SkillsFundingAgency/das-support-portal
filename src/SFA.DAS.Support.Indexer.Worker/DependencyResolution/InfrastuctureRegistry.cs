using System.Diagnostics;
using SFA.DAS.Support.Indexer.ApplicationServices.Services;
using SFA.DAS.Support.Indexer.Infrastructure.Manifest;
using StructureMap.Configuration.DSL;
using SFA.DAS.Support.Common.Infrastucture.Indexer;
using SFA.DAS.Support.Common.Infrastucture.Elasticsearch;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Reflection;
using SFA.DAS.Support.Shared.Authentication;
using SFA.DAS.Support.Shared.Discovery;
using SFA.DAS.Support.Shared.SiteConnection;

namespace SFA.DAS.Support.Indexer.Worker.DependencyResolution
{
    [ExcludeFromCodeCoverage]
    public class InfrastuctureRegistry : Registry
    {
        public InfrastuctureRegistry()
        {

            For<List<SiteManifest>>()
                .Singleton()
                .Use<List<SiteManifest>>(x =>  WorkerRole.SiteManifests);
            For<Dictionary<string, SiteChallenge>>()
                .Singleton()
                .Use<Dictionary<string, SiteChallenge>>(x =>  WorkerRole.SiteChallenges);
            For<Dictionary<string, SiteResource>>()
                .Singleton()
                .Use<Dictionary<string, SiteResource>>(x =>  WorkerRole.SiteResources);

            For<IHttpStatusCodeStrategy>().Use<StrategyForSystemErrorStatusCode>();
            For<IHttpStatusCodeStrategy>().Use<StrategyForClientErrorStatusCode>();
            For<IHttpStatusCodeStrategy>().Use<StrategyForRedirectionStatusCode>();
            For<IHttpStatusCodeStrategy>().Use<StrategyForSuccessStatusCode>();
            For<IHttpStatusCodeStrategy>().Use<StrategyForInformationStatusCode>();

            
            For<IClientAuthenticator>().Use<ActiveDirectoryClientAuthenticator>();

            For<HttpClient>().Use((c) => new HttpClient());


            For<IGetSearchItemsFromASite>().Use<ManifestProvider>();

            For<IGetSiteManifest>().Use<ManifestProvider>();
           
            For<IElasticsearchClientFactory>().Use<ElasticsearchClientFactory>();

            For<IElasticsearchCustomClient>().Use<ElasticsearchCustomClient>();

            For<IIndexProvider>().Use<ElasticSearchIndexProvider>();

            For<ISiteConnector>().Use<SiteConnector>();

            For<IClientAuthenticator>().Use<ActiveDirectoryClientAuthenticator>();

            For<IIndexSearchItems>().Use<IndexerService>();

            For<IIndexNameCreator>().Use<IndexNameCreator>();
        }

        private IDictionary<string, object> GetProperties()
        {
            var properties = new Dictionary<string, object> {{"Version", GetVersion()}};
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