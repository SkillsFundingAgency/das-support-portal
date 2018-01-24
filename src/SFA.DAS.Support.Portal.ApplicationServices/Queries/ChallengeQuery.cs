using MediatR;
using SFA.DAS.Support.Portal.ApplicationServices.Responses;

namespace SFA.DAS.Support.Portal.ApplicationServices.Queries
{
    public class ChallengeQuery : IAsyncRequest<ChallengeResponse>
    {
        public ChallengeQuery(string id)
        {
            Id = id;
        }

        public string Id { get; }
    }
}