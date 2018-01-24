using MediatR;
using SFA.DAS.Support.Portal.ApplicationServices.Responses;

namespace SFA.DAS.Support.Portal.ApplicationServices.Queries
{
    public class AccountLevySubmissionsQuery : IAsyncRequest<AccountLevySubmissionsResponse>
    {
        public AccountLevySubmissionsQuery(string id, string position)
        {
            Id = id;
            Position = position;
        }

        public string Id { get; }
        public string Position { get; }
    }
}