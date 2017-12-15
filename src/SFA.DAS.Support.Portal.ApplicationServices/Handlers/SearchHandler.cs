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

namespace SFA.DAS.Support.Portal.ApplicationServices.Handlers
{
    public class SearchHandler : IAsyncRequestHandler<SearchQuery, SearchResponse>
    {
        private readonly ISearchProvider<SearchItem> _provider;
        private readonly IManifestRepository _manifestRepository;

        private const int _pageSize = 10;

        public SearchHandler(ISearchProvider<SearchItem> provider, IManifestRepository manifestRepository)
        {
            _provider = provider;
            _manifestRepository = manifestRepository;
        }

        public async Task<SearchResponse> Handle(SearchQuery query)
        {
            query.Page = query.Page <= 0 ? 10 : query.Page;

            var searchResponse = new SearchResponse
            {
                Page = query.Page,
                SearchTerm = query.Query
            };

            var searchData = _provider.Search(query.Query, _pageSize, query.Page);
            if (searchData != null && searchData.Results != null)
            {
                var results = searchData.Results
                               .GroupBy(o => o.SearchResultCategory)
                               .ToDictionary(g => g.Key, g => g.Select(x => x.SearchResultJson).ToList());

                searchResponse.Results = results;

                var searchMetaData = await _manifestRepository.GetSearchResultsMetadata();

                searchResponse.SearchResultsMetadata = searchMetaData ?? new List<SearchResultMetadata>();
                searchResponse.LastPage = searchData.LastPage;

            }

            return searchResponse;
        }
    }
}
