using MediatR;
using SFA.DAS.Support.Portal.ApplicationServices.Responses;

namespace SFA.DAS.Support.Portal.ApplicationServices.Queries
{
    public class AccountPayeSchemesQuery : IAsyncRequest<AccountPayeSchemesResponse>
    {
        public AccountPayeSchemesQuery(string id)
        {
            Id = id;
        }

        public string Id { get; }
    }
}