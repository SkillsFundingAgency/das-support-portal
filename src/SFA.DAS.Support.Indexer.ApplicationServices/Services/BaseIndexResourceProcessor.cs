using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using Nest;
using SFA.DAS.NLog.Logger;
using SFA.DAS.Support.Common.Infrastucture.Elasticsearch;
using SFA.DAS.Support.Common.Infrastucture.Indexer;
using SFA.DAS.Support.Common.Infrastucture.Settings;
using SFA.DAS.Support.Indexer.ApplicationServices.Settings;
using SFA.DAS.Support.Shared.SearchIndexModel;
using SFA.DAS.Support.Shared.SiteConnection;

namespace SFA.DAS.Support.Indexer.ApplicationServices.Services
{
    public abstract class BaseIndexResourceProcessor<T> : IIndexResourceProcessor where T : class
    {
        private const int _indexToRetain = 5;

        protected readonly ISiteConnector _dataSource;
        protected readonly IElasticsearchCustomClient _elasticClient;
        protected readonly IIndexNameCreator _indexNameCreator;
        protected readonly IIndexProvider _indexProvider;

        protected readonly Stopwatch _indexTimer = new Stopwatch();
        protected readonly ILog _logger;
        protected readonly Stopwatch _queryTimer = new Stopwatch();
        protected readonly Stopwatch _runtimer = new Stopwatch();
        protected readonly ISearchSettings _searchSettings;
        protected readonly ISiteSettings _settings;

        public BaseIndexResourceProcessor(ISiteSettings settings,
            ISiteConnector dataSource,
            IIndexProvider indexProvider,
            ISearchSettings searchSettings,
            ILog logger,
            IIndexNameCreator indexNameCreator,
            IElasticsearchCustomClient elasticClient)
        {
            _settings = settings;
            _dataSource = dataSource;
            _indexProvider = indexProvider;
            _searchSettings = searchSettings;
            _logger = logger;
            _indexNameCreator = indexNameCreator;
            _elasticClient = elasticClient;
        }

        public async Task ProcessResource(Uri uri, SearchCategory searchCategory)
        {
            if (!ContinueProcessing(searchCategory)) return;

            try
            {
                var newIndexName = _indexNameCreator.CreateNewIndexName(_searchSettings.IndexName, searchCategory);
                CreateIndex(newIndexName);

                _queryTimer.Start();
                _logger.Info($" Downloading Index Records for type {typeof(T).Name}...");
                var searchItems = await _dataSource.Download<IEnumerable<T>>(uri);
                if (_dataSource.LastCode != HttpStatusCode.OK)
                {
                    throw _dataSource.LastException ??
                          throw new InvalidOperationException("The requested data was not recieved");
                }

                _queryTimer.Stop();

                _indexTimer.Start();
                _logger.Info($" Indexing Documents for type {typeof(T).Name}...");
                _indexProvider.IndexDocuments(newIndexName, searchItems);
                _indexTimer.Stop();

                _logger.Info($"Creating Index Alias and Swapping from old to new index for type {typeof(T).Name}...");
                var indexAlias = _indexNameCreator.CreateIndexesAliasName(_searchSettings.IndexName, searchCategory);
                _indexProvider.CreateIndexAlias(newIndexName, indexAlias);


                _logger.Info($"Deleting Old Indexes for type {typeof(T).Name}...");
                _indexProvider.DeleteIndexes(_indexToRetain, indexAlias);
                _logger.Info($"Deleting Old Indexes Completed for type {typeof(T).Name}...");

                _indexTimer.Stop();
                _queryTimer.Stop();
                _logger.Info(
                    $"Query Elapse Time For {typeof(T).Name} : {_queryTimer.Elapsed} - Indexing Time {_indexTimer.Elapsed}");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Exception while Indexing {typeof(T).Name}");
            }
        }

        protected abstract void CreateIndex(string newIndexName);

        protected abstract bool ContinueProcessing(SearchCategory searchCategory);


        protected void ValidateResponse(string indexName, ICreateIndexResponse response)
        {
            if (response.ApiCall.HttpStatusCode != (int)HttpStatusCode.OK)
                throw new Exception(
                    $"Call to ElasticSearch client Received non-200 response when trying to create the Index {indexName}, Status Code:{response.ApiCall.HttpStatusCode ?? -1}\r\n{response.DebugInformation}",
                    response.OriginalException);
        }
    }
}