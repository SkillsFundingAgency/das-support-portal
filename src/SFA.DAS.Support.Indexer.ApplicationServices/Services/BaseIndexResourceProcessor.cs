using Nest;
using SFA.DAS.NLog.Logger;
using SFA.DAS.Support.Common.Infrastucture.Elasticsearch;
using SFA.DAS.Support.Common.Infrastucture.Indexer;
using SFA.DAS.Support.Common.Infrastucture.Settings;
using SFA.DAS.Support.Indexer.ApplicationServices.Settings;
using SFA.DAS.Support.Shared.Discovery;
using SFA.DAS.Support.Shared.SearchIndexModel;
using SFA.DAS.Support.Shared.SiteConnection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;

namespace SFA.DAS.Support.Indexer.ApplicationServices.Services
{
    public abstract class BaseIndexResourceProcessor<T> : IIndexResourceProcessor where T : class
    {
        private const int IndexToRetain = 2;
        private const int PageSize = 50;
        
        protected readonly IElasticsearchCustomClient ElasticClient;
        protected readonly ISearchSettings SearchSettings;
        
        private readonly ISiteConnector _dataSource;
        private readonly IIndexNameCreator _indexNameCreator;
        private readonly IIndexProvider _indexProvider;
        private readonly Stopwatch _indexTimer = new Stopwatch();
        private readonly ILog _logger;
        private readonly Stopwatch _queryTimer = new Stopwatch();

        protected BaseIndexResourceProcessor(
            ISiteConnector dataSource,
            IIndexProvider indexProvider,
            ISearchSettings searchSettings,
            ILog logger,
            IIndexNameCreator indexNameCreator,
            IElasticsearchCustomClient elasticClient)
        {
            _dataSource = dataSource;
            _indexProvider = indexProvider;
            SearchSettings = searchSettings;
            _logger = logger;
            _indexNameCreator = indexNameCreator;
            ElasticClient = elasticClient;
        }

        public async Task ProcessResource(IndexResourceProcessorModel resourceProcessorModel)
        {
            if (!ContinueProcessing(resourceProcessorModel.SiteResource.SearchCategory)) return;

            try
            {
                var newIndexName = _indexNameCreator.CreateNewIndexName(SearchSettings.IndexName, resourceProcessorModel.SiteResource.SearchCategory);
                CreateIndex(newIndexName);

                try
                {
                    await IndexDocument(resourceProcessorModel.BasUri, resourceProcessorModel.SiteResource, newIndexName, resourceProcessorModel.ResourceIdentifier);
                }
                catch (Exception)
                {
                    _logger.Info($"Deleting New Index {newIndexName} due to exception");
                    _indexProvider.DeleteIndex(newIndexName);
                    throw;
                }

                _logger.Info($"Creating Index Alias and Swapping from old to new index for type {typeof(T).Name}...");
                var indexAlias = _indexNameCreator.CreateIndexesAliasName(SearchSettings.IndexName, resourceProcessorModel.SiteResource.SearchCategory);
                _indexProvider.CreateIndexAlias(newIndexName, indexAlias);

                _logger.Info($"Deleting Old Indexes for type {typeof(T).Name}...");
                _indexProvider.DeleteIndexes(IndexToRetain, indexAlias);
                _logger.Info($"Deleting Old Indexes Completed for type {typeof(T).Name}...");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Exception while Indexing {typeof(T).Name}");
            }
        }

        protected abstract void CreateIndex(string newIndexName);

        protected abstract bool ContinueProcessing(SearchCategory searchCategory);

        protected void ValidateResponse(string indexName, Common.Infrastucture.Elasticsearch.CreateIndexResponse response)
        {
            if (response.HttpStatusCode != (int)HttpStatusCode.OK)
                throw new Exception(
                    $"Call to ElasticSearch client Received non-200 response when trying to create the Index {indexName}, Status Code:{response.HttpStatusCode ?? -1}\r\n{response.DebugInformation}",
                    response.OriginalException);
        }

        private async Task IndexDocument(Uri baseUri, SiteResource siteResource, string newIndexName, string resourceIdentifier)
        {
            _queryTimer.Start();
            _logger.Info($" Downloading Index Records for type {typeof(T).Name}...");

            var searchItemCountUri = new Uri(baseUri, string.Format(siteResource.SearchTotalItemsUrl, 1));
            string totalSearchItemsString = null;
            var retryCount = 0;
            do
            {
                totalSearchItemsString = await _dataSource.Download(searchItemCountUri, resourceIdentifier);
            }
            while (_dataSource.LastCode == HttpStatusCode.Unauthorized && ++retryCount < 3);

            ValidateDownResponse(_dataSource.LastCode);

            if (!int.TryParse(totalSearchItemsString, out int totalSearchItems))
            {
                var errorMsg = $"Get Total Search Item Count returned invalid data from : {totalSearchItemsString}";
                throw new InvalidCastException(errorMsg);
            }

            _logger.Info($"Estimated Total Search Items Count  for type {typeof(T).Name}  equals {totalSearchItems}");

            var pages = (int)Math.Ceiling(totalSearchItems / (double)PageSize);

            for (int pageNumber = 1; pageNumber <= pages; pageNumber++)
            {
                var searchUri = new Uri(baseUri, string.Format(siteResource.SearchItemsUrl, PageSize, pageNumber));
                IEnumerable<T> searchItems;
                retryCount = 0;
                do
                {
                    searchItems = await _dataSource.Download<IEnumerable<T>>(searchUri, resourceIdentifier);
                }
                while (_dataSource.LastCode == HttpStatusCode.Unauthorized && ++retryCount < 3);

                try
                {
                    ValidateDownResponse(_dataSource.LastCode);
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, $" Error while retriving page {pageNumber} for type {typeof(T).Name}.");
                    // removed throw to allow index to be created when 1 or 2 pages fail to be retrieved.
                }

                _indexTimer.Start();
                _logger.Info($" Indexing Documents for type {typeof(T).Name}...page : {pageNumber}");
                _indexProvider.IndexDocuments(newIndexName, searchItems);
                _indexTimer.Stop();
                _logger.Info($"Indexing Time {_indexTimer.Elapsed} page : {pageNumber}");
            }

            _queryTimer.Stop();
            _logger.Info($"Query Elapse Time For {typeof(T).Name} : {_queryTimer.Elapsed}");
        }

        private void ValidateDownResponse(HttpStatusCode responseCode)
        {
            if (_dataSource.LastCode != HttpStatusCode.OK)
            {
                throw _dataSource.LastException ?? throw new InvalidOperationException("The requested data was not recieved");
            }
        }
    }
}