using System.Collections.Generic;

namespace SFA.DAS.Support.Portal.Infrastructure.Services
{
    public interface IAzureSearchProvider
    {
        IEnumerable<T> Search<T>(string searchText, int top = 50, int skip = 0) where T : class;
    }
}