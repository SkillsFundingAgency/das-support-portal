using System.Threading.Tasks;
using HMRC.ESFA.Levy.Api.Types;

namespace SFA.DAS.Support.Portal.ApplicationServices
{
    public interface ILevySubmissionsRepository
    {
        Task<LevyDeclarations> Get(string payeScheme);
    }
}