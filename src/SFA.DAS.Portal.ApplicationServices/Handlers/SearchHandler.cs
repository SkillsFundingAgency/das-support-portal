using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Portal.ApplicationServices.Queries;
using SFA.DAS.Portal.ApplicationServices.Responses;
using SFA.DAS.Portal.Core.Services;

namespace SFA.DAS.Portal.ApplicationServices.Handlers
{
    public class SearchHandler : IAsyncRequestHandler<SearchQuery, SearchResponse>
    {
        private readonly IEntityRepository _repository;

        public SearchHandler(IEntityRepository repository)
        {
            _repository = repository;
        }

        public async Task<SearchResponse> Handle(SearchQuery message)
        {
            return new SearchResponse
            {
                Results = _repository.Search(message.Query)
            };
        }
    }
}
