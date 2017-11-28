using System.Collections.Generic;

namespace SFA.DAS.Portal.Core.Services
{
    public interface IEntityRepository
    {
        IEnumerable<string> Search(string query);
    }
}
