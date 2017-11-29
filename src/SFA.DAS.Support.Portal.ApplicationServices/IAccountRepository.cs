using System.Threading.Tasks;
using SFA.DAS.Support.Portal.Core.Domain.Model;

namespace SFA.DAS.Support.Portal.ApplicationServices
{
    public interface IAccountRepository
    {
        Task<Account> Get(string id, AccountFieldsSelection selection);
        Task<decimal> GetAccountBalance(string id);
    }
}
