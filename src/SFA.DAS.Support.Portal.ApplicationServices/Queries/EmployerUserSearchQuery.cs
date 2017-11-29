using MediatR;
using SFA.DAS.Support.Portal.ApplicationServices.Responses;

namespace SFA.DAS.Support.Portal.ApplicationServices.Queries
{
    public class EmployerUserSearchQuery : IAsyncRequest<EmployerUserSearchResponse>
    {
        public string SearchTerm { get; set; }

        public int Page { get; set; }
    }
}