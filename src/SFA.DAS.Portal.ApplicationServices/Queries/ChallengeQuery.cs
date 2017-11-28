using MediatR;
using SFA.DAS.Portal.ApplicationServices.Responses;

namespace SFA.DAS.Portal.ApplicationServices.Queries
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