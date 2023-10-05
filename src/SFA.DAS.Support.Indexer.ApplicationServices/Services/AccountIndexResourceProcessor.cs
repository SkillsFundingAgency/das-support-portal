using SFA.DAS.NLog.Logger;
using SFA.DAS.Support.Common.Infrastucture.Elasticsearch;
using SFA.DAS.Support.Common.Infrastucture.Indexer;
using SFA.DAS.Support.Common.Infrastucture.Settings;
using SFA.DAS.Support.Shared.SearchIndexModel;
using SFA.DAS.Support.Shared.SiteConnection;

namespace SFA.DAS.Support.Indexer.ApplicationServices.Services
{
    public class AccountIndexResourceProcessor : BaseIndexResourceProcessor<AccountSearchModel>
    {
        public AccountIndexResourceProcessor(
            ISiteConnector downloader,
            IIndexProvider indexProvider,
            ISearchSettings searchSettings,
            ILog logger,
            IIndexNameCreator indexNameCreator,
            IElasticsearchCustomClient elasticClient)
            : base(downloader,
                indexProvider,
                searchSettings,
                logger,
                indexNameCreator,
                elasticClient)
        {
        }

        protected override bool ContinueProcessing(SearchCategory searchCategory)
        {
            return searchCategory == SearchCategory.Account;
        }

        protected override void CreateIndex(string indexName)
        {
            if (ElasticClient.IndexExists(indexName, string.Empty))
            {
                return;
            }
            
            var response = ElasticClient.CreateIndex(
                indexName,
                i => i
                    .Settings(settings =>
                        settings
                            .NumberOfShards(SearchSettings.IndexShards)
                            .NumberOfReplicas(SearchSettings.IndexReplicas)
                    )
                    .Mappings(ms => ms
                        .Map<AccountSearchModel>(m => m
                            .Properties(p => p
                                .Text(k => k.Name(n => n.Account))
                                .Keyword(k => k.Name(n => n.AccountID))
                                .Keyword(k => k.Name(n => n.PublicAccountID))
                                .Keyword(k => k.Name(n => n.AccountSearchKeyWord))
                                .Keyword(k => k.Name(n => n.AccountIDSearchKeyWord))
                                .Keyword(k => k.Name(n => n.PublicAccountIDSearchKeyWord))
                                .Keyword(k => k.Name(n => n.PayeSchemeIdSearchKeyWords))
                            )))
                , string.Empty);

            ValidateResponse(indexName, response);
        }
    }
}