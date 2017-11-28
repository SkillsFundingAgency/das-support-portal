using MediatR;
using SFA.DAS.Portal.ApplicationServices.Responses;

namespace SFA.DAS.Portal.ApplicationServices.Queries
{
    public class SearchQuery : IAsyncRequest<SearchResponse>
    {
        public string Query { get; set; }
    }
}
