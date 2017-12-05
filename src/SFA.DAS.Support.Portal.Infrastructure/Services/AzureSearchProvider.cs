using System.Collections.Generic;
using System.Linq;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using SFA.DAS.Support.Shared;

namespace SFA.DAS.Support.Portal.Infrastructure.Services
{

    public class AzureSearchProvider : ISearchProvider
    {
        private readonly ISearchIndexClient _client;

        public AzureSearchProvider(ISearchIndexClient client)
        {
            _client = client;
        }

        public IEnumerable<SearchItem> Search<SeachItem>(string searchText, int top = 50, int skip = 0)
        {
            var searchParameters = new SearchParameters
            {
                Skip = skip,
                Top = top
            };

            searchParameters.Skip = skip;
            var response = _client.Documents.Search<SearchItem>(searchText, searchParameters);
            foreach (var document in response.Results.Select(x => x.Document))
            {
                yield return document;
            }
        }
    }
}
