using System.Threading.Tasks;
using SFA.DAS.Portal.ApplicationServices.Queries;
using SFA.DAS.Portal.Core.Domain.Model;

namespace SFA.DAS.Portal.ApplicationServices
{
    public interface IChallengeRepository
    {
        Task<bool> CheckData(Account record, ChallengePermissionQuery message);
    }
}