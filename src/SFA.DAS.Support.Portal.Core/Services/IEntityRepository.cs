using SFA.DAS.Support.Portal.Core.Domain.Model;
using System.Collections.Generic;

namespace SFA.DAS.Support.Portal.Core.Services
{
    public interface IEntityRepository
    {
        IEnumerable<GenericSearchResult> Search(string query);
    }
}
