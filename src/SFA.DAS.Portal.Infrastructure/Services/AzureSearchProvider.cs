using System.Collections.Generic;
using System.Linq;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;

namespace SFA.DAS.Portal.Infrastructure.Services
{
    public interface IAzureSearchProvider
    {
        IEnumerable<T> Search<T>(string searchText, int top = 50, int skip = 0) where T : class;
    }

    public class AzureSearchProvider : IAzureSearchProvider
    {
        private readonly ISearchIndexClient _client;

        public AzureSearchProvider(ISearchIndexClient client)
        {
            _client = client;
        }

        public IEnumerable<T> Search<T>(string searchText, int top = 50, int skip = 0) where T : class
        {
            var searchParameters = new SearchParameters
            {
                Skip = skip,
                Top = top
            };

            searchParameters.Skip = skip;
            var response = _client.Documents.Search<T>(searchText, searchParameters);
            foreach (var document in response.Results.Select(x => x.Document))
            {
                yield return document;
            }
        }
    }
}
