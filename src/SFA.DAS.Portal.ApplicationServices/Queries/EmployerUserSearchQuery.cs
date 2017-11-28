using MediatR;
using SFA.DAS.Portal.ApplicationServices.Responses;

namespace SFA.DAS.Portal.ApplicationServices.Queries
{
    public class EmployerUserSearchQuery : IAsyncRequest<EmployerUserSearchResponse>
    {
        public string SearchTerm { get; set; }

        public int Page { get; set; }
    }
}