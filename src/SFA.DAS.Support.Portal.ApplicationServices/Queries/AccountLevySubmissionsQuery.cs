using MediatR;
using SFA.DAS.Support.Portal.ApplicationServices.Responses;

namespace SFA.DAS.Support.Portal.ApplicationServices.Queries
{
    public class AccountLevySubmissionsQuery : IAsyncRequest<AccountLevySubmissionsResponse>
    {
        public string Id { get; private set; }
        public string Position { get; private set; }
        
        public AccountLevySubmissionsQuery(string id, string position)
        {
            Id = id;
            Position = position;
        }
    }
}