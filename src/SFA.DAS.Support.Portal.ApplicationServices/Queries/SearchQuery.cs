using System.Diagnostics.CodeAnalysis;
using MediatR;
using SFA.DAS.Support.Portal.ApplicationServices.Responses;

namespace SFA.DAS.Support.Portal.ApplicationServices.Queries
{
    //[ExcludeFromCodeCoverage]
    public class SearchQuery : IAsyncRequest<SearchResponse>
    {
        public string Query { get; set; }
    }
}
