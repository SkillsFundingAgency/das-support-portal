using SFA.DAS.Support.Portal.Core.Domain.Model;

namespace SFA.DAS.Support.Portal.ApplicationServices.Responses
{
    public class EmployerUserResponse
    {
        public EmployerUser User { get; set; }
        public SearchResponseCodes StatusCode { get; set; }
    }
}
