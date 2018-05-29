using System;
using System.Linq;
using System.Net;
using Nest;
using SFA.DAS.Support.Common.Infrastucture.Elasticsearch.Exceptions;
using SFA.DAS.Support.Common.Infrastucture.Indexer;
using SFA.DAS.Support.Common.Infrastucture.Models;
using SFA.DAS.Support.Common.Infrastucture.Settings;
using SFA.DAS.Support.Shared.SearchIndexModel;

namespace SFA.DAS.Support.Common.Infrastucture.Elasticsearch
{
    public class ElasticSearchProvider : ISearchProvider
    {
        private readonly IElasticsearchCustomClient _elasticSearchClient;
        private readonly IIndexNameCreator _indexNameCreator;
        private readonly ISearchSettings _searchSettings;

        private string _indexAliasName;

        public ElasticSearchProvider(IElasticsearchCustomClient elasticSearchClient,
            ISearchSettings searchSettings,
            IIndexNameCreator indexNameCreator)
        {
            _elasticSearchClient = elasticSearchClient;
            _searchSettings = searchSettings;
            _indexNameCreator = indexNameCreator;
        }

        public PagedSearchResponse<UserSearchModel> FindUsers(string searchText, SearchCategory searchType, int pageSize = 10, int pageNumber = 1)
        {
            if (searchType != SearchCategory.User) return null;

            _indexAliasName = _indexNameCreator.CreateIndexesAliasName(_searchSettings.IndexName, searchType);

            var response = _elasticSearchClient.Search<UserSearchModel>(s => s.Index(_indexAliasName)
                 .Type(Types.Type<UserSearchModel>())
                 .Skip(pageSize * GetPage(pageNumber))
                 .Take(pageSize)
                 .Query(q => q
                 .Bool(b => b
                 .Should(m =>
                     m.QueryString(qs => qs.Query($"*{searchText}*").AnalyzeWildcard(true).Fields(f => f.Field(fs => fs.FirstNameSearchKeyWord)))
                      ||
                     m.QueryString(qs => qs.Query($"*{searchText}*").AnalyzeWildcard(true).Fields(f => f.Field(fs => fs.LastNameSearchKeyWord)))
                      ||
                     m.QueryString(qs => qs.Query($"*{searchText}*").AnalyzeWildcard(true).Fields(f => f.Field(fs => fs.EmailSearchKeyWord)))
                      ||
                     m.QueryString(qs => qs.Query($"*{searchText}*").AnalyzeWildcard(true).Fields(f => f.Field(fs => fs.NameSearchKeyWord)))
                      ||
                     m.Match(mt => mt.Query(searchText).Field(fs => fs.FirstNameSearchKeyWord))
                      ||
                     m.Match(mt => mt.Query(searchText).Field(fs => fs.LastNameSearchKeyWord))
                      ||
                     m.Match(mt => mt.Query(searchText).Field(fs => fs.NameSearchKeyWord))
                      ||
                     m.Match(mt => mt.Query(searchText).Field(fs => fs.EmailSearchKeyWord))
                 )
                 ))
                 .Sort(sort => sort.Descending(SortSpecialField.Score))

                , string.Empty);


            ValidateResponse(response);

            return GetSearchResponse(pageSize, response);
        }


        public PagedSearchResponse<AccountSearchModel> FindAccounts(string searchText, SearchCategory searchType, int pageSize = 10, int pageNumber = 1)
        {
            if (searchType != SearchCategory.Account) return null;
            _indexAliasName = _indexNameCreator.CreateIndexesAliasName(_searchSettings.IndexName, searchType);

            var response = _elasticSearchClient.Search<AccountSearchModel>(s => s.Index(_indexAliasName)
                .Type(Types.Type<AccountSearchModel>())
                .Skip(pageSize * GetPage(pageNumber))
                .Take(pageSize)
                .Query(q => q
                .Bool(b => b
                .Should(m =>
                      m.QueryString(qs => qs.Query($"*{searchText }*").AnalyzeWildcard(true).Fields(f => f.Field(fs => fs.Account)))
                        ||
                        m.QueryString(qs => qs.Query($"*{searchText }*").AnalyzeWildcard(true).Fields(f => f.Field(fs => fs.AccountSearchKeyWord)))
                         ||
                        m.Match(mt => mt.Query(searchText).Field(fs => fs.AccountSearchKeyWord))
                        ||
                        m.Match(mt => mt.Query(searchText).Field(fs => fs.AccountIDSearchKeyWord))
                        ||
                        m.Match(mt => mt.Query(searchText).Field(fs => fs.PublicAccountIDSearchKeyWord))
                        ||
                        m.Match(mt => mt.Query(searchText).Field(fs => fs.PayeSchemeIdSearchKeyWords))
                )))
                 .Sort(sort => sort.Descending(SortSpecialField.Score).Ascending(a => a.AccountSearchKeyWord))
                   , string.Empty);

            ValidateResponse(response);

            return GetSearchResponse(pageSize, response);
        }

        private PagedSearchResponse<T> GetSearchResponse<T>(int pageSize, ISearchResponse<T> response) where T : class
        {

            var responsePageSize = pageSize == 0 ? 1 : pageSize;
            var lastPage = (int)(response.Total / responsePageSize);

            return new PagedSearchResponse<T>
            {
                LastPage = lastPage < 0 ? 0 : lastPage,
                TotalCount = response.Total,
                Results = response?.Documents?.Select(d => d).ToList()
            };
        }

        private int GetPage(int pageNumber)
        {
            return pageNumber < 0 ? 0 : pageNumber;
        }

        private void ValidateResponse<T>(ISearchResponse<T> response) where T : class
        {
            if (response?.ApiCall.HttpStatusCode != (int)HttpStatusCode.OK)
            {

                var errorMsg = $"Received non-200 response Status Code:{response.ApiCall.HttpStatusCode.GetValueOrDefault()} \r\n" +
                                $"Reasons:{ string.Join("\r\n", response.ServerError?.Error?.RootCause)}";

                throw new ElasticSearchInvalidResponseException(errorMsg, response.OriginalException);
            }

        }


    }
}