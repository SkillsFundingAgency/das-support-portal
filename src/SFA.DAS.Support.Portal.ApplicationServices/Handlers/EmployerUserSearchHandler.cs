using System.Linq;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Support.Portal.ApplicationServices.Mapping;
using SFA.DAS.Support.Portal.ApplicationServices.Queries;
using SFA.DAS.Support.Portal.ApplicationServices.Responses;

namespace SFA.DAS.Support.Portal.ApplicationServices.Handlers
{
    public class EmployerUserSearchHandler : IAsyncRequestHandler<EmployerUserSearchQuery, EmployerUserSearchResponse>
    {
        private readonly IEmployerUserRepository _repository;

        private readonly UserSummaryMapper _mapper;

        public EmployerUserSearchHandler(IEmployerUserRepository repository)
        {
            _repository = repository;
            _mapper = new UserSummaryMapper();
        }

        public async Task<EmployerUserSearchResponse> Handle(EmployerUserSearchQuery message)
        {
            message.Page = message.Page <= 0 ? 1 : message.Page;

            var response = new EmployerUserSearchResponse
            {
                SearchTerm = message.SearchTerm,
                Page = message.Page
            };

            var searchResults = await _repository.Search(message.SearchTerm, message.Page);

            if (searchResults != null)
            {
                response.StatusCode = searchResults.Results.Any() ? SearchResponseCodes.Success : SearchResponseCodes.NoSearchResultsFound;
                response.Results = searchResults.Results.Select(x => _mapper.MapFromEmployerUserSummary(x));
                response.LastPage = searchResults.LastPage;

                return response;
            }

            response.StatusCode = SearchResponseCodes.SearchFailed;
            return response;
        }


        
    }
}
