using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Support.Portal.ApplicationServices.Queries;
using SFA.DAS.Support.Portal.ApplicationServices.Responses;
using SFA.DAS.Support.Portal.Core.Services;
using SFA.DAS.Support.Portal.ApplicationServices.Services;

namespace SFA.DAS.Support.Portal.ApplicationServices.Handlers
{
    public class SearchHandler : IAsyncRequestHandler<SearchQuery, SearchResponse>
    {
        private readonly IEntityRepository _repository;
        private readonly IManifestRepository _manifestRepository;

        public SearchHandler(IEntityRepository repository, IManifestRepository manifestRepository)
        {
            _repository = repository;
        }

        public async Task<SearchResponse> Handle(SearchQuery message)
        {
            var searchResponse = new SearchResponse();

            var searchData = _repository.Search(message.Query);
            if (searchData != null)
            {
                var results = searchData
                               .GroupBy(o => o.SearchResultCategory)
                               .ToDictionary(g => g.Key, g => g.Select(x => x.SearchResultJson).ToList());

                searchResponse.Results = results;
            }

            var searchMetaData = await _manifestRepository.GetSearchResultsMetadata();
            searchResponse.SearchResultsMetadata = searchMetaData ?? new List<Shared.SearchResultMetadata>();

            return await Task.FromResult(searchResponse);


        }
    }
}
