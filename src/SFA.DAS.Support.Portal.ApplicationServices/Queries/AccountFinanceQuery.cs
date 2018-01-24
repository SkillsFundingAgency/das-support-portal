using MediatR;
using SFA.DAS.Support.Portal.ApplicationServices.Responses;

namespace SFA.DAS.Support.Portal.ApplicationServices.Queries
{
    public class AccountFinanceQuery : IAsyncRequest<AccountFinanceResponse>
    {
        public AccountFinanceQuery(string id)
        {
            Id = id;
        }

        public string Id { get; }
    }
}