using MediatR;
using SFA.DAS.Support.Portal.ApplicationServices.Responses;

namespace SFA.DAS.Support.Portal.ApplicationServices.Queries
{
    public class AccountFinanceQuery : IAsyncRequest<AccountFinanceResponse>
    {
        public string Id { get; private set; }

        public AccountFinanceQuery(string id)
        {
            Id = id;
        }
    }
}