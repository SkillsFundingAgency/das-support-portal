using MediatR;
using SFA.DAS.Support.Portal.ApplicationServices.Responses;

namespace SFA.DAS.Support.Portal.ApplicationServices.Queries
{
    public class EmployerUserQuery : IAsyncRequest<EmployerUserResponse>
    {
        public string Id { get; private set; }

        public EmployerUserQuery(string id)
        {
            Id = id;
        }
    }
}
