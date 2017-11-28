using System.Collections.Generic;

namespace SFA.DAS.Support.Portal.Core.Services
{
    public interface IEntityRepository
    {
        IEnumerable<string> Search(string query);
    }
}
