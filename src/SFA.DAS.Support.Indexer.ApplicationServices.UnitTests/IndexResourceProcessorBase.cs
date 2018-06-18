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

        protected Mock<ISiteConnector> _downloader;
        protected Mock<IIndexProvider> _indexProvider;
        protected Mock<ISearchSettings> _searchSettings;
        protected Mock<ILog> _logger;
        protected Mock<IIndexNameCreator> _indexNameCreator;
        protected Mock<IElasticsearchCustomClient> _elasticClient;
        protected Mock<ISiteSettings> _siteSettings;

        protected const string _indexName = "new_index_name";
        protected Uri _baseUrl;
        protected const int _indexToRetain = 2;
        protected IEnumerable<AccountSearchModel> _accountModels;
        protected SiteResource _accountSiteResource;
        protected SiteResource _userSiteResource;


        protected void Initialise()
        {
            _baseUrl = new Uri("http://localhost");

            _accountModels = new List<AccountSearchModel>
            {
                new AccountSearchModel
                {
                    Account = "Valtech"
                }
            };


            _accountSiteResource = new SiteResource
            {
                SearchCategory = SearchCategory.Account,
                SearchTotalItemsUrl = "localhost",
                SearchItemsUrl = "localhost",
            };

            _userSiteResource = new SiteResource
            {
                SearchCategory = SearchCategory.User,
                SearchTotalItemsUrl = "localhost",
                SearchItemsUrl = "localhost",
            };


            _downloader = new Mock<ISiteConnector>();
            _indexProvider = new Mock<IIndexProvider>();

            _searchSettings = new Mock<ISearchSettings>();
            _searchSettings.Setup(o => o.IndexName).Returns(_indexName);


            _logger = new Mock<ILog>();
            _indexNameCreator = new Mock<IIndexNameCreator>();
            _elasticClient = new Mock<IElasticsearchCustomClient>();
            _siteSettings = new Mock<ISiteSettings>();

            var existsResponse = new Mock<IExistsResponse>();

            existsResponse
                .SetupGet(o => o.Exists)
                .Returns(false);

            _indexNameCreator
                .Setup(o => o.CreateNewIndexName(_indexName, SearchCategory.Account))
                .Returns(_indexName);

            _indexNameCreator
                .Setup(o => o.CreateIndexesAliasName(_indexName, SearchCategory.Account))
                .Returns("new_index_name_Alias");

            _elasticClient
                .Setup(o => o.IndexExists(_indexName, string.Empty))
                .Returns(existsResponse.Object);

            var createIndexResponse = new Mock<ICreateIndexResponse>();
            createIndexResponse
                .Setup(o => o.ApiCall.HttpStatusCode)
                .Returns((int)HttpStatusCode.OK);

            _elasticClient
               .Setup(x => x.CreateIndex(_indexName, It.IsAny<Func<CreateIndexDescriptor, ICreateIndexRequest>>(), string.Empty))
               .Returns(createIndexResponse.Object);

            _downloader
                .Setup(o => o.Download<IEnumerable<AccountSearchModel>>(_baseUrl))
                .Returns(Task.FromResult(_accountModels));

            _downloader
               .Setup(o => o.Download(It.IsAny<Uri>()))
               .Returns(Task.FromResult("50"));

            _downloader
                .Setup(o => o.LastCode)
                .Returns(HttpStatusCode.OK);

            _indexProvider
                .Setup(o => o.DeleteIndex(_indexName));

            _indexProvider
               .Setup(o => o.CreateIndexAlias(_indexName, It.IsAny<string>()));

            _indexProvider
              .Setup(o => o.DeleteIndexes(_indexToRetain, It.IsAny<string>()));

           
        }




    }
}
