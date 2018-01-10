using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Support.Portal.ApplicationServices.Queries;
using SFA.DAS.Support.Portal.ApplicationServices.Responses;
using SFA.DAS.Support.Portal.Core.Services;
using SFA.DAS.Support.Portal.ApplicationServices.Services;
using SFA.DAS.Support.Shared;
using SFA.DAS.Support.Common.Infrastucture.Indexer;
using SFA.DAS.Support.Shared.SearchIndexModel;
using SFA.DAS.Support.Common.Infrastucture.Models;

namespace SFA.DAS.Support.Portal.ApplicationServices.Handlers
{
    public class SearchHandler : IAsyncRequestHandler<SearchQuery, SearchResponse>
    {
        private readonly ISearchProvider _searchProvider;
        private const int _pageSize = 10;

        public SearchHandler(ISearchProvider searchProvider)
        {
            _searchProvider = searchProvider;
        }

        public async Task<SearchResponse> Handle(SearchQuery query)
        {
 
            var searchResponse = new SearchResponse
            {
                Page = query.Page,
                SearchTerm = query.SearchTerm,
                SearchType = query.SearchType
            };

            var userResponse = _searchProvider.FindUsers(query.SearchTerm, query.SearchType, _pageSize, query.Page);
            searchResponse.UserSearchResult = userResponse;

            var accountResponse = _searchProvider.FindAccounts(query.SearchTerm, query.SearchType, _pageSize, query.Page);
            searchResponse.AccountSearchResult = accountResponse;
            
            return await Task.FromResult(searchResponse);
        }
    }
}
