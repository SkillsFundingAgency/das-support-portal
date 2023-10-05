using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.NLog.Logger;
using SFA.DAS.Support.Common.Infrastucture.Elasticsearch.Exceptions;
using SFA.DAS.Support.Common.Infrastucture.Indexer;
using SFA.DAS.Support.Portal.ApplicationServices.Queries;
using SFA.DAS.Support.Portal.ApplicationServices.Responses;

namespace SFA.DAS.Support.Portal.ApplicationServices.Handlers
{
    public class SearchHandler : IRequestHandler<SearchQuery, SearchResponse>
    {
        private readonly ILog _log;

        private readonly ISearchProvider _searchProvider;

        public SearchHandler(ISearchProvider searchProvider, ILog log)
        {
            _searchProvider = searchProvider;
            _log = log;
        }

        public async Task<SearchResponse> Handle(SearchQuery query, CancellationToken cancellationToken)
        {
            var searchResponse = new SearchResponse
            {
                Page = query.Page,
                SearchTerm = query.SearchTerm,
                SearchType = query.SearchType
            };

            try
            {
                query.SearchTerm = query.SearchTerm.Replace("/", string.Empty).Replace("*", string.Empty).ToLower();

                if (!string.IsNullOrWhiteSpace(query.SearchTerm))
                {

                    var userResponse =
                        _searchProvider.FindUsers(query.SearchTerm, query.SearchType, query.PageSize, query.Page);
                    searchResponse.UserSearchResult = userResponse;

                    var accountResponse =
                        _searchProvider.FindAccounts(query.SearchTerm, query.SearchType, query.PageSize, query.Page);
                    searchResponse.AccountSearchResult = accountResponse;
                }

            }
            catch (ElasticSearchInvalidResponseException ex)
            {
                _log.Error(ex,
                    $"Error while searching for {query.SearchTerm} search type {query.SearchType} on Page {query.Page}");
            }

            return await Task.FromResult(searchResponse);
        }

    }
}