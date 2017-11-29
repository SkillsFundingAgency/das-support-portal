using System.Threading.Tasks;
using SFA.DAS.Support.Portal.Health.Model;

namespace SFA.DAS.Support.Portal.Health
{
    public interface IHealthService
    {
        Task<HealthModel> CreateHealthModel();

        Task<HealthEmployerUserModel> CreateHealthEmployerUserModel();

        Task<HealthAccountsModel> CreateHealthAccountsModel();
    }
}