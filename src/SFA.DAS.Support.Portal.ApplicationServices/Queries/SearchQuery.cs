using MediatR;
using SFA.DAS.Support.Portal.ApplicationServices.Responses;

namespace SFA.DAS.Support.Portal.ApplicationServices.Queries
{
    public class SearchQuery : IAsyncRequest<SearchResponse>
    {
        public string Query { get; set; }
    }
}
