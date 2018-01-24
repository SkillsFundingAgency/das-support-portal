using MediatR;
using SFA.DAS.Support.Portal.ApplicationServices.Responses;

namespace SFA.DAS.Support.Portal.ApplicationServices.Queries
{
    public class AccountDetailOrganisationsQuery : IAsyncRequest<AccountDetailOrganisationsResponse>
    {
        public AccountDetailOrganisationsQuery(string id)
        {
            Id = id;
        }

        public string Id { get; }
    }
}