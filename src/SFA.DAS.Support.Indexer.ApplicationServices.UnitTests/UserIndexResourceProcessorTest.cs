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
            IndexNameCreator
                .Setup(o => o.CreateNewIndexName(IndexName, SearchCategory.Account))
                .Returns("new_index_name");

            var sut = new UserIndexResourceProcessor(Downloader.Object,
                                                        IndexProvider.Object,
                                                        SearchSettings.Object,
                                                        Logger.Object,
                                                        IndexNameCreator.Object,
                                                        ElasticClient.Object);

            await sut.ProcessResource(new IndexResourceProcessorModel
            {
                BasUri = new System.Uri("http://localhost"),
                SiteResource = AccountSiteResource,
                ResourceIdentifier = ResourceIdentifier
            });

            IndexNameCreator.Verify(o => o.CreateNewIndexName(IndexName, SearchCategory.Account), Times.Never);
        }
    }
}