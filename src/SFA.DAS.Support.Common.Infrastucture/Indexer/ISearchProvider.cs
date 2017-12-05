using SFA.DAS.Support.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFA.DAS.Support.Portal.Infrastructure.Services
{
    public interface ISearchProvider
    {
        IEnumerable<SearchItem> Search<SeachItem>(string searchText, int top = 50, int skip = 0);
    }
}
