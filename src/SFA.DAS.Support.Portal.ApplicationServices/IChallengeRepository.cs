using System.Threading.Tasks;
using SFA.DAS.Support.Portal.ApplicationServices.Queries;
using SFA.DAS.Support.Portal.Core.Domain.Model;

namespace SFA.DAS.Support.Portal.ApplicationServices
{
    public interface IChallengeRepository
    {
        Task<bool> CheckData(Account record, ChallengePermissionQuery message);
    }
}