using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Portal.ApplicationServices.Queries;
using SFA.DAS.Portal.ApplicationServices.Responses;

namespace SFA.DAS.Portal.ApplicationServices.Handlers
{
    public class EmployerUserDetailHandler : IAsyncRequestHandler<EmployerUserQuery, EmployerUserResponse>
    {
        private readonly IEmployerUserRepository _repository;

        public EmployerUserDetailHandler(IEmployerUserRepository repository)
        {
            _repository = repository;
        }

        public async Task<EmployerUserResponse> Handle(EmployerUserQuery message)
        {
            var response = new EmployerUserResponse
            {
                StatusCode = SearchResponseCodes.NoSearchResultsFound
            };

            var record = message.Id != null ?  await _repository.Get(message.Id) : null;

            if (record != null)
            {
                response.StatusCode = SearchResponseCodes.Success;
                response.User = record;
            }

            return response;
        }
    }
}