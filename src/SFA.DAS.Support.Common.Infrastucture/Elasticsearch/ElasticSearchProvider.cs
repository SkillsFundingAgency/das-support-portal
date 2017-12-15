using Nest;
using SFA.DAS.Support.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using SFA.DAS.Support.Common.Infrastucture.Indexer;
using SFA.DAS.Support.Common.Infrastucture.Models;

namespace SFA.DAS.Support.Common.Infrastucture.Elasticsearch
{
    public class ElasticSearchProvider : ISearchProvider<SearchItem>
    {
        private IElasticsearchCustomClient _elasticSearchClient;
        private readonly string _indexAliasName;

        public ElasticSearchProvider(IElasticsearchCustomClient elasticSearchClient, string indexAliasName)
        {
            _elasticSearchClient = elasticSearchClient;
            _indexAliasName = indexAliasName;
        }

        public PagedSearchResponse<SearchItem> Search(string searchText, int pageSize = 10, int pageNumber = 0)
        {

            var response = _elasticSearchClient.Search<SearchItem>(s => s.Index(_indexAliasName)
                                                       .Type(Types.Type<SearchItem>())
                                                       .Skip(pageSize * pageNumber)
                                                       .Take(pageSize)
                                                       .Query(q => q
                                                       .MatchPhrasePrefix(mp => mp
                                                       .Query(searchText)
                                                       .Field(f => f.Keywords))), string.Empty);

            var countResponse = _elasticSearchClient.Count<SearchItem>(c => c.Index(_indexAliasName)     
                                                    .Type(Types.Type<SearchItem>())
                                                    .Query(q => q
                                                    .MatchPhrasePrefix(mp => mp
                                                    .Query(searchText)
                                                    .Field(f => f.Keywords))),string.Empty);

            if (response?.ApiCall.HttpStatusCode != (int)HttpStatusCode.OK  || countResponse?.ApiCall.HttpStatusCode != (int)HttpStatusCode.OK)
            {
                throw new Exception($"Received non-200 response when trying to fetch the search items from elastic serach Index, Status Code:{response.ApiCall.HttpStatusCode}");
            }

            var totalcount = countResponse == null ? 0 : countResponse.Count;


            return new PagedSearchResponse<SearchItem>
            {
                LastPage = totalcount <= 0 ? 1 : (int)(totalcount/pageSize),
                TotalCount = totalcount,
                Results= response.Documents.Select(d => d).ToList()
            };
            
        }
        
    }

}

