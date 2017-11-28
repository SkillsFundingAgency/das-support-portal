using MediatR;
using SFA.DAS.Portal.ApplicationServices.Responses;

namespace SFA.DAS.Portal.ApplicationServices.Queries
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
