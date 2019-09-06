using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Nest;

namespace SFA.DAS.Support.Common.Infrastucture.Elasticsearch
{
    public class DeleteIndexResponse
    {
        public bool Acknowledged { get; set; }
        public Exception OriginalException { get; internal set; }
    }

    public class IndicesStatsResponse
    {
        public IReadOnlyDictionary<string, IndicesStats> Indices { get; set; }
    }

    public class CountResponse
    {
        public long Count { get; set; }
    }

    public interface IElasticsearchCustomClient
    {
        ISearchResponse<T> Search<T>(Func<SearchDescriptor<T>, ISearchRequest> selector,
            [CallerMemberName] string callerName = "")
            where T : class;

        CountResponse Count<T>(Func<CountDescriptor<T>, ICountRequest> selector,
            [CallerMemberName] string callerName = "") where T : class;

        bool IndexExists(IndexName index, [CallerMemberName] string callerName = "");

        DeleteIndexResponse DeleteIndex(IndexName index, [CallerMemberName] string callerName = "");

        GetMappingResponse GetMapping<T>(Func<GetMappingDescriptor<T>, IGetMappingRequest> selector = null,
            [CallerMemberName] string callerName = "")
            where T : class;

        RefreshResponse Refresh(IRefreshRequest request, [CallerMemberName] string callerName = "");

        RefreshResponse Refresh(Indices indices, Func<RefreshDescriptor, IRefreshRequest> selector = null,
            [CallerMemberName] string callerName = "");

        bool AliasExists(string aliasName,
            [CallerMemberName] string callerName = "");

        BulkAliasResponse Alias(string aliasName, string indexName, [CallerMemberName] string callerName = "");

        BulkAliasResponse Alias(IBulkAliasRequest request, [CallerMemberName] string callerName = "");

        IndicesStatsResponse IndicesStats(Indices indices,
            Func<IndicesStatsDescriptor, IIndicesStatsRequest> selector = null,
            [CallerMemberName] string callerName = "");

        IList<string> GetIndicesPointingToAlias(string aliasName, [CallerMemberName] string callerName = "");

        CreateIndexResponse CreateIndex(IndexName index,
            Func<CreateIndexDescriptor, ICreateIndexRequest> selector = null,
            [CallerMemberName] string callerName = "");

        Task<BulkResponse> BulkAsync(IBulkRequest request, [CallerMemberName] string callerName = "");

        void BulkAll<T>(IEnumerable<T> documents, string indexName, int batchSize) where T : class;
    }
}