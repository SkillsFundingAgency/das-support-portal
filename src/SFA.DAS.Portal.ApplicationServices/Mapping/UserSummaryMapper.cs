using SFA.DAS.Portal.Core.Domain.Model;

namespace SFA.DAS.Portal.ApplicationServices.Mapping
{
    public class UserSummaryMapper
    {
        public UserSummary MapFromEmployerUserSummary(EmployerUserSummary employerUserSummary)
        {
            return new UserSummary
            {
                Id = employerUserSummary.Id,
                FirstName = employerUserSummary.FirstName,
                LastName = employerUserSummary.LastName,
                Email = employerUserSummary.Email,
                Status = employerUserSummary.Status,
                Href = employerUserSummary.Href,
                Accounts = employerUserSummary.Accounts
            };
        }
    }
}
