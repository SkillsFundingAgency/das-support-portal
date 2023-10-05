using SFA.DAS.NLog.Logger;
using SFA.DAS.Support.Common.Infrastucture.Elasticsearch;
using SFA.DAS.Support.Common.Infrastucture.Indexer;
using SFA.DAS.Support.Common.Infrastucture.Settings;
using SFA.DAS.Support.Shared.SearchIndexModel;
using SFA.DAS.Support.Shared.SiteConnection;

namespace SFA.DAS.Support.Indexer.ApplicationServices.Services
{
    public class UserIndexResourceProcessor : BaseIndexResourceProcessor<UserSearchModel>
    {
        public UserIndexResourceProcessor(
            ISiteConnector downloader,
            IIndexProvider indexProvider,
            ISearchSettings searchSettings,
            ILog logger,
            IIndexNameCreator indexNameCreator,
            IElasticsearchCustomClient elasticClient) 
            : base(downloader, indexProvider, searchSettings, logger, indexNameCreator, elasticClient)
        {
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
                        .Map<UserSearchModel>(m => m
                            .Properties(p => p
                                .Keyword(k => k.Name(n => n.Id))
                                .Keyword(k => k.Name(n => n.Status))

                                .Keyword(k => k.Name(n => n.Email))
                                .Keyword(k => k.Name(n => n.FirstName))
                                .Keyword(k => k.Name(n => n.LastName))
                                .Keyword(k => k.Name(n => n.Name))

                                .Keyword(k => k.Name(n => n.EmailSearchKeyWord))
                                .Keyword(k => k.Name(n => n.FirstNameSearchKeyWord))
                                .Keyword(k => k.Name(n => n.LastNameSearchKeyWord))
                                .Keyword(k => k.Name(n => n.NameSearchKeyWord))
                            )))
                , string.Empty);

            ValidateResponse(indexName, response);
        }

        protected override bool ContinueProcessing(SearchCategory searchCategory)
        {
            return searchCategory == SearchCategory.User;
        }
    }
}
