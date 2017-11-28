using MediatR;
using SFA.DAS.Portal.ApplicationServices.Responses;

namespace SFA.DAS.Portal.ApplicationServices.Queries
{
    public class AccountDetailOrganisationsQuery : IAsyncRequest<AccountDetailOrganisationsResponse>
    {
        public string Id { get; private set; }

        public AccountDetailOrganisationsQuery(string id)
        {
            Id = id;
        }
    }
}