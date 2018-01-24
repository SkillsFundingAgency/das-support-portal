using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Elasticsearch.Net;
using Nest;
using SFA.DAS.NLog.Logger;

namespace SFA.DAS.Support.Common.Infrastucture.Elasticsearch
{
    public class ElasticsearchCustomClient : IElasticsearchCustomClient
    {
        private readonly IElasticClient _client;
        private readonly ILog _logger;

        public ElasticsearchCustomClient(IElasticsearchClientFactory elasticsearchClientFactory, ILog logger)
        {
            _client = elasticsearchClientFactory.GetElasticClient();
            _logger = logger;
        }

        public ISearchResponse<T> Search<T>(Func<SearchDescriptor<T>, ISearchRequest> selector,
            [CallerMemberName] string callerName = "")
            where T : class
        {
            var timer = Stopwatch.StartNew();
            var result = _client.Search(selector);

            SendLog(result.ApiCall, result.Took, timer.ElapsedMilliseconds, $"Search : {callerName}");
            return result;
        }

        public ICountResponse Count<T>(Func<CountDescriptor<T>, ICountRequest> selector,
            [CallerMemberName] string callerName = "")
            where T : class
        {
            var timer = Stopwatch.StartNew();
            var result = _client.Count(selector);

            SendLog(result.ApiCall, null, timer.ElapsedMilliseconds, $"Search : {callerName}");
            return result;
        }

        public IExistsResponse IndexExists(IndexName index, [CallerMemberName] string callerName = "")
        {
            var timer = Stopwatch.StartNew();
            IExistsResponse result = null;
            try
            {
                result = _client.IndexExists(index);
            }
            catch (Exception e)
            {
                _logger.Debug($"IndexExists: {e.Message}");
            }

            SendLog(result.ApiCall, null, timer.ElapsedMilliseconds, $"Index Exists {index.Name}");
            return result;
        }

        public IDeleteIndexResponse DeleteIndex(IndexName index, [CallerMemberName] string callerName = "")
        {
            var timer = Stopwatch.StartNew();
            var result = _client.DeleteIndex(index);
            SendLog(result.ApiCall, null, timer.ElapsedMilliseconds, $"Delete Index {index.Name}");
            return result;
        }

        public IGetMappingResponse GetMapping<T>(Func<GetMappingDescriptor<T>, IGetMappingRequest> selector = null,
            [CallerMemberName] string callerName = "")
            where T : class
        {
            var timer = Stopwatch.StartNew();
            var result = _client.GetMapping(selector);
            SendLog(result.ApiCall, null, timer.ElapsedMilliseconds, $"Get Mapping {callerName}");
            return result;
        }

        public IRefreshResponse Refresh(IRefreshRequest request, [CallerMemberName] string callerName = "")
        {
            var timer = Stopwatch.StartNew();
            var result = _client.Refresh(request);
            SendLog(result.ApiCall, null, timer.ElapsedMilliseconds, $"Refresh {callerName}");
            return result;
        }

        public IRefreshResponse Refresh(Indices indices, Func<RefreshDescriptor, IRefreshRequest> selector = null,
            string callerName = "")
        {
            var timer = Stopwatch.StartNew();
            var result = _client.Refresh(indices);
            SendLog(result.ApiCall, null, timer.ElapsedMilliseconds, $"Refresh {callerName}");
            return result;
        }

        public IExistsResponse AliasExists(Func<AliasExistsDescriptor, IAliasExistsRequest> selector,
            string callerName = "")
        {
            var timer = Stopwatch.StartNew();
            var result = _client.AliasExists(selector);
            SendLog(result.ApiCall, null, timer.ElapsedMilliseconds, $"Alias Exists {callerName}");
            return result;
        }

        public IBulkAliasResponse Alias(string aliasName, string indexName, string callerName = "")
        {
            Func<BulkAliasDescriptor, IBulkAliasRequest> selector = a =>
                a.Add(add => add.Index(indexName).Alias(aliasName));
            var timer = Stopwatch.StartNew();
            var result = _client.Alias(selector);
            SendLog(result.ApiCall, null, timer.ElapsedMilliseconds, $"Alias {aliasName} > {indexName}");
            return result;
        }

        public IBulkAliasResponse Alias(IBulkAliasRequest request, string callerName = "")
        {
            var timer = Stopwatch.StartNew();
            var result = _client.Alias(request);
            SendLog(result.ApiCall, null, timer.ElapsedMilliseconds, $"Alias {callerName}");
            return result;
        }

        public IIndicesStatsResponse IndicesStats(Indices indices,
            Func<IndicesStatsDescriptor, IIndicesStatsRequest> selector = null, string callerName = "")
        {
            var timer = Stopwatch.StartNew();
            var result = _client.IndicesStats(indices, selector);
            SendLog(result.ApiCall, null, timer.ElapsedMilliseconds, $"Indices Stats {callerName}");
            return result;
        }

        public IList<string> GetIndicesPointingToAlias(string aliasName, string callerName = "")
        {
            var timer = Stopwatch.StartNew();
            var result = _client.GetIndicesPointingToAlias(aliasName);
            SendLog(null, null, timer.ElapsedMilliseconds, $"Get Indices Pointing To Alias {aliasName}");
            return result.ToList();
        }

        public ICreateIndexResponse CreateIndex(IndexName index,
            Func<CreateIndexDescriptor, ICreateIndexRequest> selector = null, string callerName = "")
        {
            var timer = Stopwatch.StartNew();
            var result = _client.CreateIndex(index, selector);
            SendLog(result.ApiCall, null, timer.ElapsedMilliseconds, $"Create Index {index.Name}");
            return result;
        }

        public virtual Task<IBulkResponse> BulkAsync(IBulkRequest request, string callerName = "")
        {
            var timer = Stopwatch.StartNew();
            var result = _client.BulkAsync(request);
            SendLog(null, null, timer.ElapsedMilliseconds, $"Bulk Async {callerName}");
            return result;
        }

        public void BulkAll<T>(IEnumerable<T> documents, string indexName, int batchSize) where T : class
        {
            var count = 0;
            var elementCount = documents.Count();
            var timer = Stopwatch.StartNew();
            var waitHandle = new CountdownEvent(1);

            var bulkAll = _client.BulkAll(documents, b => b
                .Index(indexName)
                .BackOffRetries(15)
                .BackOffTime(TimeSpan.FromSeconds(30))
                .RefreshOnCompleted(true)
                .MaxDegreeOfParallelism(4)
                .Size(batchSize));

            bulkAll.Subscribe(new BulkAllObserver(
                b =>
                {
                    count = count + batchSize;

                    if (count > elementCount) count = elementCount;

                    _logger.Debug($"Indexed group of Document: {count} of {elementCount}");
                },
                e =>
                {
                    _logger.Error(e, e.Message);
                    throw e;
                },
                () => { waitHandle.Signal(); }));
            waitHandle.Wait();

            SendLog(null, null, timer.ElapsedMilliseconds, "Bulk completed for Document");
        }

        private void SendLog(IApiCallDetails apiCallDetails, long? took, double networkTime, string identifier)
        {
            var body = string.Empty;
            if (apiCallDetails?.RequestBodyInBytes != null)
                body = Encoding.Default.GetString(apiCallDetails.RequestBodyInBytes);

            var properties = new Dictionary<string, object>
            {
                {"ReturnCode", apiCallDetails?.HttpStatusCode},
                {"SearchTime", took},
                {"NetworkTime", networkTime},
                {"Url", apiCallDetails?.Uri?.AbsoluteUri},
                {"Body", body}
            };

            _logger.Debug($"Elasticsearch {identifier}", properties);
        }
    }
}