using Moq;
using NUnit.Framework;
using SFA.DAS.NLog.Logger;
using SFA.DAS.Support.Common.Infrastucture.Elasticsearch;
using SFA.DAS.Support.Common.Infrastucture.Indexer;
using SFA.DAS.Support.Common.Infrastucture.Settings;
using SFA.DAS.Support.Indexer.ApplicationServices.Services;
using SFA.DAS.Support.Shared.SiteConnection;
using SFA.DAS.Support.Indexer.ApplicationServices;
using SFA.DAS.Support.Indexer.ApplicationServices.Settings;
using SFA.DAS.Support.Shared.SearchIndexModel;
using System.Threading.Tasks;

namespace SFA.DAS.Support.Indexer.ApplicationServices.UnitTests
{
    [TestFixture]
    public class UserIndexResourceProcessorTest : IndexResourceProcessorBase
    {
       
        [SetUp]
        public void Setup()
        {
            Initialise();
        }


        [Test]
        public async Task ShouldProcessOnlyUserSearchType()
        {

            _indexNameCreator
                .Setup(o => o.CreateNewIndexName(_indexName, SearchCategory.Account))
                .Returns("new_index_name");


            var _sut = new UserIndexResourceProcessor(_siteSettings.Object,
                                                        _downloader.Object,
                                                        _indexProvider.Object,
                                                        _searchSettings.Object,
                                                        _logger.Object,
                                                        _indexNameCreator.Object,
                                                        _elasticClient.Object);

            await _sut.ProcessResource(new System.Uri("http://localhost"), SearchCategory.Account);

            _indexNameCreator
                .Verify(o => o.CreateNewIndexName(_indexName, SearchCategory.Account), Times.Never);

        }


    }
}
