using System;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.NLog.Logger;
using SFA.DAS.Support.Common.Infrastucture.Elasticsearch.Exceptions;
using SFA.DAS.Support.Common.Infrastucture.Indexer;
using SFA.DAS.Support.Portal.ApplicationServices.Queries;
using SFA.DAS.Support.Portal.ApplicationServices.Responses;

namespace SFA.DAS.Support.Portal.ApplicationServices.Handlers
{
    public class SearchHandler : IAsyncRequestHandler<SearchQuery, SearchResponse>
    {
        private const int _pageSize = 10;
        private readonly ISearchProvider _searchProvider;
        private readonly ILog _log;

        public SearchHandler(ISearchProvider searchProvider, ILog log)
        {
            _searchProvider = searchProvider;
            _log = log;
        }

        public async Task<SearchResponse> Handle(SearchQuery query)
        {
            var searchResponse = new SearchResponse
            {
                Page = query.Page,
                SearchTerm = query.SearchTerm,
                SearchType = query.SearchType
            };

            try
            {
                //TODO configure ES analyser
                query.SearchTerm = query.SearchTerm.ToLower();

                var userResponse = _searchProvider.FindUsers(query.SearchTerm, query.SearchType, _pageSize, query.Page);
                searchResponse.UserSearchResult = userResponse;

                var accountResponse = _searchProvider.FindAccounts(query.SearchTerm, query.SearchType, _pageSize, query.Page);
                searchResponse.AccountSearchResult = accountResponse;
            }
            catch (ElasticSearchInvalidResponseException ex)
            {
                _log.Error(ex, $"Error while searching for { query.SearchTerm} search type {query.SearchType} on Page {query.Page}");
            }

            return await Task.FromResult(searchResponse);
        }
    }
}