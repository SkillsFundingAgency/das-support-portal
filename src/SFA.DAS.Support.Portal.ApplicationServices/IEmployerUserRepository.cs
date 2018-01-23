using System.Threading.Tasks;
using SFA.DAS.Support.Portal.ApplicationServices.Models;
using SFA.DAS.Support.Portal.Core.Domain.Model;

namespace SFA.DAS.Support.Portal.ApplicationServices
{
    public interface IEmployerUserRepository
    {
        Task<EmployerUserSearchResults> Search(string searchTerm, int page);

        Task<EmployerUser> Get(string id);
    }
}