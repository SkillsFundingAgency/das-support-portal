using MediatR;
using SFA.DAS.Portal.ApplicationServices.Responses;

namespace SFA.DAS.Portal.ApplicationServices.Queries
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