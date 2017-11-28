using System.Threading.Tasks;
using SFA.DAS.Portal.Core.Domain.Model;

namespace SFA.DAS.Portal.ApplicationServices
{
    public interface IAccountRepository
    {
        Task<Account> Get(string id, AccountFieldsSelection selection);
        Task<decimal> GetAccountBalance(string id);
    }
}
