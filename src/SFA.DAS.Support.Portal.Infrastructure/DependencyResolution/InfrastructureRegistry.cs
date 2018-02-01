using System.Diagnostics.CodeAnalysis;
using SFA.DAS.Support.Common.Infrastucture.Elasticsearch;
using SFA.DAS.Support.Common.Infrastucture.Indexer;
using SFA.DAS.Support.Portal.ApplicationServices.Services;
using SFA.DAS.Support.Portal.Infrastructure.Services;
using SFA.DAS.Support.Shared.SiteConnection;
using StructureMap.Configuration.DSL;

namespace SFA.DAS.Support.Portal.Infrastructure.DependencyResolution
{
    [ExcludeFromCodeCoverage]
    public class InfrastructureRegistry : Registry
    {
        public InfrastructureRegistry()
        {
            For<IIndexNameCreator>().Use<IndexNameCreator>();
            For<IElasticsearchClientFactory>().Use<ElasticsearchClientFactory>();
            For<IElasticsearchCustomClient>().Use<ElasticsearchCustomClient>();
            For<IIndexNameCreator>().Use<IndexNameCreator>();
            For<ISearchProvider>().Use<ElasticSearchProvider>();
            For<ISiteConnector>().Use<SiteConnector>();
            For<IFormMapper>().Use<FormMapper>();
            For<IWindowsLogonIdentityProvider>().Use<WindowsLogonIdentityProvider>();
        }
        
    }
}