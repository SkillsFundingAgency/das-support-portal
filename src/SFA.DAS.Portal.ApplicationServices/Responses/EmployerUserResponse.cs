using SFA.DAS.Portal.Core.Domain.Model;

namespace SFA.DAS.Portal.ApplicationServices.Responses
{
    public class EmployerUserResponse
    {
        public EmployerUser User { get; set; }
        public SearchResponseCodes StatusCode { get; set; }
    }
}
