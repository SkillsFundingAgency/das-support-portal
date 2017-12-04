using Nest;
using SFA.DAS.Support.Portal.Infrastructure.Services;
using SFA.DAS.Support.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;


namespace SFA.DAS.Support.Common.Infrastucture.Elasticsearch
{
    public class ElasticSearchProvider : ISearchProvider
    {

        private IElasticsearchCustomClient _elasticSearchClient;
        private readonly string _indexAliasName;

        public ElasticSearchProvider(IElasticsearchCustomClient elasticSearchClient, string indexAliasName)
        {
            _elasticSearchClient = elasticSearchClient;
            _indexAliasName = indexAliasName;
        }

        public IEnumerable<SearchItem> Search<SeachItem>(string searchText, int top = 50, int skip = 0)
        {
            var response = _elasticSearchClient.Search<SearchItem>(s =>
                     s.Index(_indexAliasName)
                         .Type(Types.Type<SearchItem>())
                         .Skip(skip)
                         .Take(100)
                           .Query(q => q
                           .MatchPhrasePrefix(mp => mp
                           .Query(searchText)
                           .Field(f => f.Keywords)
                        )),string.Empty);

         
            if (response?.ApiCall.HttpStatusCode != (int)HttpStatusCode.OK)
            {
                throw new Exception($"Received non-200 response when trying to fetch the search items from elastic serach Index, Status Code:{response.ApiCall.HttpStatusCode}");
            }

            return response.Documents.Select(x => x);
        }

      
    }

}

