using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Nest;
using SFA.DAS.NLog.Logger;
using SFA.DAS.Support.Common.Infrastucture.Indexer;
using SFA.DAS.Support.Common.Infrastucture.Settings;

namespace SFA.DAS.Support.Common.Infrastucture.Elasticsearch
{
    public class ElasticSearchIndexProvider : IIndexProvider
    {
        private const int _defautltIndexCopyCount = 3;
        private readonly IElasticsearchCustomClient _client;
        private readonly ILog _logger;
        private readonly ISearchSettings _settings;

        public ElasticSearchIndexProvider(IElasticsearchCustomClient elasticsearchCustomClient, ILog logger,
            ISearchSettings settings)
        {
            _client = elasticsearchCustomClient;
            _logger = logger;
            _settings = settings;
        }

        public void CreateIndex<T>(string indexName) where T : class
        {
            if (!_client.IndexExists(indexName, string.Empty).Exists)
            {
                var response = _client.CreateIndex(
                    indexName,
                    i => i
                        .Settings(settings =>
                            settings
                                .NumberOfShards(_settings.IndexShards)
                                .NumberOfReplicas(_settings.IndexReplicas)
                        )
                        .Mappings(ms => ms
                            .Map<T>(m => m
                                .AutoMap()
                                .Properties(p => p)))
                    , string.Empty);

                if (response.ApiCall.HttpStatusCode != (int) HttpStatusCode.OK)
                    throw new Exception(
                        $"Call to ElasticSearch client Received non-200 response when trying to create the Index {nameof(indexName)}, Status Code:{response.ApiCall.HttpStatusCode ?? -1}\r\n{response.DebugInformation}",
                        response.OriginalException);
            }
        }

        public void IndexDocuments<T>(string indexName, IEnumerable<T> documents) where T : class
        {
            try
            {
                Console.WriteLine($"Indexed {documents?.Count()} Documents");

                _client.BulkAll(documents, indexName, 1000);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Failed to index all documents Error: {e.Message}");
            }

            Console.WriteLine($"{documents?.Count()} Documents indexed sucessfully");
        }

        public void DeleteIndex(string indexName)
        {
            var result = _client.DeleteIndex(indexName, string.Empty);

            if (result == null || !result.Acknowledged)
            {
                var msg = $"Unable to delete Index {indexName}";
                _logger.Error(result.OriginalException, msg);
            }
        }

        public void DeleteIndexes(int indexToRetain, string indexPrefix)
        {
            indexToRetain = indexToRetain <= 0 ? _defautltIndexCopyCount : indexToRetain;

            var indexNameDelimeter = new[] {'_'};

            var indexToDeleteCount = _client
                .IndicesStats(Indices.All, null, string.Empty)
                .Indices
                .Count(x => x.Key.StartsWith(indexPrefix) && x.Key.Split(indexNameDelimeter).Count() == 2);

            if (indexToDeleteCount > indexToRetain)
            {
                var indicesToBeDelete = _client
                    .IndicesStats(Indices.All, null, string.Empty)
                    .Indices
                    .Where(x => x.Key.StartsWith(indexPrefix))
                    .OrderByDescending(x => x.Key.Split(indexNameDelimeter).Last())
                    .Skip(indexToRetain)
                    .ToList();

                _logger.Debug($"Deleting {indicesToBeDelete.Count()} indexes...");

                foreach (var index in indicesToBeDelete)
                {
                    _logger.Debug($"Deleting {index.Key}");
                    DeleteIndex(index.Key);
                }
            }

            _logger.Debug("Deletion completed...");
        }

        public bool IndexExists(string indexName)
        {
            return _client.IndexExists(indexName, string.Empty).Exists;
        }

        public void CreateIndexAlias(string newIndexName, string aliasName)
        {
            if (!_client.AliasExists(a => a.Name(aliasName), string.Empty).Exists)
            {
                _logger.Warn("Alias doesn't exist, creating a new one...");

                _client.Alias(aliasName, newIndexName, string.Empty);
            }
            else
            {
                SwapAliasIndex(aliasName, newIndexName);
            }
        }

        private void SwapAliasIndex(string aliasName, string newIndexName)
        {
            var existingIndexesOnAlias = _client.GetIndicesPointingToAlias(aliasName, string.Empty);
            var aliasRequest = new BulkAliasRequest {Actions = new List<IAliasAction>()};

            foreach (var existingIndexOnAlias in existingIndexesOnAlias)
                aliasRequest.Actions.Add(new AliasRemoveAction
                {
                    Remove = new AliasRemoveOperation {Alias = aliasName, Index = existingIndexOnAlias}
                });

            aliasRequest.Actions.Add(new AliasAddAction
            {
                Add = new AliasAddOperation {Alias = aliasName, Index = newIndexName}
            });

            _client.Alias(aliasRequest, string.Empty);
        }
    }
}