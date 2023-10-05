﻿using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using SFA.DAS.Support.Common.Infrastucture.Elasticsearch;
using SFA.DAS.Support.Common.Infrastucture.Indexer;
using SFA.DAS.Support.Indexer.ApplicationServices.Services;
using SFA.DAS.Support.Shared.Authentication;
using SFA.DAS.Support.Shared.SiteConnection;
using SFA.DAS.Support.Shared.SiteConnection.Authentication;
using StructureMap.Configuration.DSL;

namespace SFA.DAS.Support.Indexer.Worker.DependencyResolution
{
    [ExcludeFromCodeCoverage]
    public class InfrastructureRegistry : Registry
    {
        public InfrastructureRegistry()
        {
            For<IHttpStatusCodeStrategy>().Use<StrategyForSystemErrorStatusCode>();
            For<IHttpStatusCodeStrategy>().Use<StrategyForClientErrorStatusCode>();
            For<IHttpStatusCodeStrategy>().Use<StrategyForRedirectionStatusCode>();
            For<IHttpStatusCodeStrategy>().Use<StrategyForSuccessStatusCode>();
            For<IHttpStatusCodeStrategy>().Use<StrategyForInformationStatusCode>();

            For<IClientAuthenticator>().Use<ActiveDirectoryClientAuthenticator>();

            For<HttpClient>().Use(c => new HttpClient());

            For<IElasticsearchClientFactory>().Use<ElasticsearchClientFactory>();

            For<IElasticsearchCustomClient>().Use<ElasticsearchCustomClient>();

            For<IIndexProvider>().Use<ElasticSearchIndexProvider>();

            For<ISiteConnector>().Use<SiteConnector>();

            For<IClientAuthenticator>().Use<ActiveDirectoryClientAuthenticator>();

            For<IIndexSearchItems>().Use<IndexerService>();

            For<IIndexNameCreator>().Use<IndexNameCreator>();

            For<IIndexResourceProcessor>()
                .Use<CompositIndexResourceProcessor>()
                .EnumerableOf<IIndexResourceProcessor>()
                .Contains(x =>
                {
                    x.Type<AccountIndexResourceProcessor>();
                    x.Type<UserIndexResourceProcessor>();
                });

            For<IAzureClientCredentialHelper>().Use<AzureClientCredentialHelper>();
        }
    }
}