using System.Threading.Tasks;
using SFA.DAS.Portal.ApplicationServices.Models;
using SFA.DAS.Portal.Core.Domain.Model;

namespace SFA.DAS.Portal.ApplicationServices
{
    public interface IEmployerUserRepository
    {
        Task<EmployerUserSearchResults> Search(string searchTerm, int page);

        Task<EmployerUser> Get(string id);
    }
}
