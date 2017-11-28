using MediatR;
using SFA.DAS.Support.Portal.ApplicationServices.Responses;

namespace SFA.DAS.Support.Portal.ApplicationServices.Queries
{
    public class ChallengeQuery : IAsyncRequest<ChallengeResponse>
    {
        public string Id { get; private set; }

        public ChallengeQuery(string id)
        {
            Id = id;
        }
    }
}