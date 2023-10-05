using Moq;
using NUnit.Framework;
using SFA.DAS.NLog.Logger;
using SFA.DAS.Support.Common.Infrastucture.Elasticsearch;
using SFA.DAS.Support.Common.Infrastucture.Indexer;
using SFA.DAS.Support.Common.Infrastucture.Settings;
using SFA.DAS.Support.Indexer.ApplicationServices.Services;
using SFA.DAS.Support.Shared.SiteConnection;
using SFA.DAS.Support.Indexer.ApplicationServices;
using SFA.DAS.Support.Indexer.ApplicationServices.Settings;
using SFA.DAS.Support.Shared.SearchIndexModel;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using Nest;
using System.Net;
using SFA.DAS.Support.Shared.Discovery;

namespace SFA.DAS.Support.Indexer.ApplicationServices.UnitTests
{
    public class IndexResourceProcessorBase
    {
        protected Mock<ISiteConnector> Downloader;
        protected Mock<IIndexProvider> IndexProvider;
        protected Mock<ISearchSettings> SearchSettings;
        protected Mock<ILog> Logger;
        protected Mock<IIndexNameCreator> IndexNameCreator;
        protected Mock<IElasticsearchCustomClient> ElasticClient;

        protected const string IndexName = "new_index_name";
        protected Uri BaseUrl;
        protected string ResourceIdentifier;

        protected const int IndexToRetain = 2;
        protected IEnumerable<AccountSearchModel> AccountModels;
        protected SiteResource AccountSiteResource;
        protected SiteResource UserSiteResource;

        protected void Initialise()
        {
            BaseUrl = new Uri("http://localhost");
            ResourceIdentifier = "https://das-test";

            AccountModels = new List<AccountSearchModel>
            {
                new AccountSearchModel
                {
                    Account = "Valtech"
                }
            };

            AccountSiteResource = new SiteResource
            {
                SearchCategory = SearchCategory.Account,
                SearchTotalItemsUrl = "localhost",
                SearchItemsUrl = "localhost",
            };

            UserSiteResource = new SiteResource
            {
                SearchCategory = SearchCategory.User,
                SearchTotalItemsUrl = "localhost",
                SearchItemsUrl = "localhost",
            };

            Downloader = new Mock<ISiteConnector>();
            IndexProvider = new Mock<IIndexProvider>();

            SearchSettings = new Mock<ISearchSettings>();
            SearchSettings.Setup(o => o.IndexName).Returns(IndexName);

            Logger = new Mock<ILog>();
            IndexNameCreator = new Mock<IIndexNameCreator>();
            ElasticClient = new Mock<IElasticsearchCustomClient>();

            IndexNameCreator
                .Setup(o => o.CreateNewIndexName(IndexName, SearchCategory.Account))
                .Returns(IndexName);

            IndexNameCreator
                .Setup(o => o.CreateIndexesAliasName(IndexName, SearchCategory.Account))
                .Returns("new_index_name_Alias");

            ElasticClient
                .Setup(o => o.IndexExists(IndexName, string.Empty))
                .Returns(false);

            ElasticClient
               .Setup(x => x.CreateIndex(IndexName, It.IsAny<Func<CreateIndexDescriptor, ICreateIndexRequest>>(), string.Empty))
               .Returns(new Common.Infrastucture.Elasticsearch.CreateIndexResponse { HttpStatusCode = (int)HttpStatusCode.OK });

            Downloader
                .Setup(o => o.Download<IEnumerable<AccountSearchModel>>(BaseUrl, It.IsAny<string>()))
                .Returns(Task.FromResult(AccountModels));

            Downloader
               .Setup(o => o.Download(It.IsAny<Uri>(), It.IsAny<string>()))
               .Returns(Task.FromResult("50"));

            Downloader
                .Setup(o => o.LastCode)
                .Returns(HttpStatusCode.OK);

            IndexProvider
                .Setup(o => o.DeleteIndex(IndexName));

            IndexProvider
               .Setup(o => o.CreateIndexAlias(IndexName, It.IsAny<string>()));

            IndexProvider
              .Setup(o => o.DeleteIndexes(IndexToRetain, It.IsAny<string>()));
        }
    }
}