using System.Threading.Tasks;
using SFA.DAS.Portal.Health.Model;

namespace SFA.DAS.Portal.Health
{
    public interface IHealthService
    {
        Task<HealthModel> CreateHealthModel();

        Task<HealthEmployerUserModel> CreateHealthEmployerUserModel();

        Task<HealthAccountsModel> CreateHealthAccountsModel();
    }
}