using Nest;
using SFA.DAS.Support.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using SFA.DAS.Support.Common.Infrastucture.Indexer;
using SFA.DAS.Support.Common.Infrastucture.Models;
using SFA.DAS.Support.Shared.SearchIndexModel;
using SFA.DAS.Support.Common.Infrastucture.Settings;
using SFA.DAS.Support.Portal.Core.Configuration;

namespace SFA.DAS.Support.Common.Infrastucture.Elasticsearch
{
    public class ElasticSearchProvider : ISearchProvider
    {
        private readonly IElasticsearchCustomClient _elasticSearchClient;
        private readonly IConfigurationSettings _configurationSettings;
        private readonly ISearchSettings _searchSettings;
        private readonly IIndexNameCreator _indexNameCreator;

        private string _indexAliasName;

        public ElasticSearchProvider(IElasticsearchCustomClient elasticSearchClient,
                                    IConfigurationSettings configurationSettings,
                                    ISearchSettings searchSettings,
                                    IIndexNameCreator indexNameCreator)
        {
            _elasticSearchClient = elasticSearchClient;
            _configurationSettings = configurationSettings;
            _searchSettings = searchSettings;
            _indexNameCreator = indexNameCreator;
        }

        public PagedSearchResponse<UserSearchModel> FindUsers(string searchText, SearchCategory searchType, int pageSize = 10, int pageNumber = 0)
        {
            if (searchType != SearchCategory.User) return null;

            _indexAliasName = _indexNameCreator.CreateIndexesAliasName(_searchSettings.IndexNameFormat, _configurationSettings.EnvironmentName, searchType);

            var response = _elasticSearchClient.Search<UserSearchModel>(s => s.Index(_indexAliasName)
                                                       .Type(Types.Type<UserSearchModel>())
                                                        .Skip(pageSize * pageNumber )
                                                       .Take(pageSize)
                                                       .Query(q => q
                                                       .MultiMatch(mp => mp
                                                       .Query(searchText)
                                                       .Fields(f => f.Field(x => x.Name)))), string.Empty);

            var countResponse = _elasticSearchClient.Count<UserSearchModel>(c => c.Index(_indexAliasName)
                                                       .Type(Types.Type<UserSearchModel>())
                                                       .Query(q => q
                                                       .MultiMatch(mp => mp
                                                       .Query(searchText)
                                                       .Fields(f => f.Field(x => x.Name)))), string.Empty);


            if (response?.ApiCall.HttpStatusCode != (int)HttpStatusCode.OK || countResponse?.ApiCall.HttpStatusCode != (int)HttpStatusCode.OK)
            {
                throw new Exception($"Received non-200 response when trying to fetch the search items from elastic serach Index, Status Code:{response.ApiCall.HttpStatusCode}");
            }

            var totalcount = countResponse == null ? 0 : countResponse.Count;
            pageSize = pageSize == 0 ? 1 : pageSize;

            return new PagedSearchResponse<UserSearchModel>
            {
                LastPage = (int)(totalcount / pageSize),
                TotalCount = totalcount,
                Results = response.Documents.Select(d => d).ToList()
            };
        }

        public PagedSearchResponse<AccountSearchModel> FindAccounts(string searchText, SearchCategory searchType, int pageSize = 10, int pageNumber = 0)
        {
            if (searchType != SearchCategory.Account) return null;
            _indexAliasName = _indexNameCreator.CreateIndexesAliasName(_searchSettings.IndexNameFormat, _configurationSettings.EnvironmentName, searchType);

            var response = _elasticSearchClient.Search<AccountSearchModel>(s => s.Index(_indexAliasName)
                                                       .Type(Types.Type<AccountSearchModel>())
                                                       .Skip(pageSize * pageNumber)
                                                       .Take(pageSize)
                                                       .Query(q => q
                                                       .MultiMatch(mp => mp
                                                       .Query(searchText)
                                                       .Fields(f => f
                                                       .Field(x => x.Account)))), string.Empty);

            var countResponse = _elasticSearchClient.Count<AccountSearchModel>(c => c.Index(_indexAliasName)
                                                    .Type(Types.Type<AccountSearchModel>())
                                                    .Query(q => q
                                                    .MultiMatch(mp => mp
                                                    .Query(searchText)
                                                    .Fields(f => f
                                                    .Field(x => x.Account)))), string.Empty);


            if (response?.ApiCall.HttpStatusCode != (int)HttpStatusCode.OK || countResponse?.ApiCall.HttpStatusCode != (int)HttpStatusCode.OK)
            {
                throw new Exception($"Received non-200 response when trying to fetch the search items from elastic serach Index, Status Code:{response.ApiCall.HttpStatusCode}");
            }

            var totalcount = countResponse == null ? 0 : countResponse.Count;
            pageSize = pageSize == 0 ? 1 : pageSize;

            return new PagedSearchResponse<AccountSearchModel>
            {
                LastPage =  (int)(totalcount / pageSize),
                TotalCount = totalcount,
                Results = response.Documents.Select(d => d).ToList()
            };
        }
    }
}

