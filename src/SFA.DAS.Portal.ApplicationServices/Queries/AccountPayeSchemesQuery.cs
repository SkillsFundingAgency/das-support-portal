using MediatR;
using SFA.DAS.Portal.ApplicationServices.Responses;

namespace SFA.DAS.Portal.ApplicationServices.Queries
{
    public class AccountPayeSchemesQuery : IAsyncRequest<AccountPayeSchemesResponse>
    {
        public string Id { get; private set; }

        public AccountPayeSchemesQuery(string id)
        {
            Id = id;
        }
    }
}