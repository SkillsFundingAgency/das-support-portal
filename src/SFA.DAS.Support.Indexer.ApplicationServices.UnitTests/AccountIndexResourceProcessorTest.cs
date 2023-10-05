using Moq;
using NUnit.Framework;
using SFA.DAS.Support.Indexer.ApplicationServices.Services;
using SFA.DAS.Support.Shared.SearchIndexModel;
using System.Threading.Tasks;
using Nest;
using System.Collections.Generic;
using System.Net;
using System;
using SFA.DAS.Support.Shared.Discovery;

namespace SFA.DAS.Support.Indexer.ApplicationServices.UnitTests
{
    [TestFixture]
    public class AccountIndexResourceProcessorTest : IndexResourceProcessorBase
    {
        [SetUp]
        public void Setup()
        {
            base.Initialise();
        }

        [Test]
        public async Task ShouldProcessOnlyAccountSearchType()
        {
            IndexNameCreator
                .Setup(o => o.CreateNewIndexName(IndexName, SearchCategory.User))
                .Returns(IndexName);

            var sut = new AccountIndexResourceProcessor(Downloader.Object,
                                                        IndexProvider.Object,
                                                        SearchSettings.Object,
                                                        Logger.Object,
                                                        IndexNameCreator.Object,
                                                        ElasticClient.Object);

            await sut.ProcessResource(new IndexResourceProcessorModel
            {
                BasUri = BaseUrl,
                SiteResource = UserSiteResource,
                ResourceIdentifier = ResourceIdentifier
            });

            IndexNameCreator
                .Verify(o => o.CreateNewIndexName(IndexName, SearchCategory.User), Times.Never);
        }

        [Test]
        public async Task ShouldNotProcessAccountModelIfUnauthorised()
        {
            Downloader
                .Setup(o => o.Download<IEnumerable<AccountSearchModel>>(BaseUrl, It.IsAny<string>()))
                .Returns(Task.FromResult(AccountModels));

            Downloader
                .Setup(o => o.Download(It.IsAny<Uri>(), It.IsAny<string>()))
                .Returns(Task.FromResult(null as string));

            Downloader
                .Setup(o => o.LastCode)
                .Returns(HttpStatusCode.Unauthorized);

            var sut = new AccountIndexResourceProcessor(
                Downloader.Object,
                IndexProvider.Object,
                SearchSettings.Object,
                Logger.Object,
                IndexNameCreator.Object,
                ElasticClient.Object);

            await sut.ProcessResource(new IndexResourceProcessorModel
            {
                BasUri = BaseUrl,
                SiteResource = AccountSiteResource,
                ResourceIdentifier = ResourceIdentifier
            });

            IndexNameCreator
                .Verify(o => o.CreateNewIndexName(IndexName, SearchCategory.Account), Times.Once);

            IndexNameCreator
                .Verify(o => o.CreateIndexesAliasName(IndexName, SearchCategory.Account), Times.Never);

            ElasticClient
                .Verify(o => o.IndexExists(IndexName, string.Empty), Times.Once);

            Downloader
                .Verify(o => o.Download(It.IsAny<Uri>(), It.IsAny<string>()), Times.Exactly(3));

            IndexProvider
                .Verify(o => o.DeleteIndex(IndexName), Times.Once);

            IndexProvider
                .Verify(o => o.CreateIndexAlias(IndexName, It.IsAny<string>()), Times.Never);

            IndexProvider
                .Verify(o => o.DeleteIndexes(IndexToRetain, It.IsAny<string>()), Times.Never);

            ElasticClient
                .Verify(x => x.CreateIndex(IndexName, It.IsAny<Func<CreateIndexDescriptor, ICreateIndexRequest>>(), string.Empty), Times.Once);
        }

        [Test]
        public async Task ShouldProcessAccountModel()
        {
            var sut = new AccountIndexResourceProcessor(Downloader.Object,
                                                        IndexProvider.Object,
                                                        SearchSettings.Object,
                                                        Logger.Object,
                                                        IndexNameCreator.Object,
                                                        ElasticClient.Object);

            await sut.ProcessResource(new IndexResourceProcessorModel
            {
                BasUri = BaseUrl,
                SiteResource = AccountSiteResource,
                ResourceIdentifier = ResourceIdentifier
            });

            IndexNameCreator
                .Verify(o => o.CreateNewIndexName(IndexName, SearchCategory.Account), Times.Once);

            IndexNameCreator
                .Verify(o => o.CreateIndexesAliasName(IndexName, SearchCategory.Account), Times.Once);

            ElasticClient
                .Verify(o => o.IndexExists(IndexName, string.Empty), Times.Once);

            Downloader
                .Verify(o => o.Download<IEnumerable<AccountSearchModel>>(It.IsAny<Uri>(), It.IsAny<string>()), Times.AtLeastOnce);

            IndexProvider
                .Verify(o => o.DeleteIndex(IndexName), Times.Never);

            IndexProvider
               .Verify(o => o.CreateIndexAlias(IndexName, It.IsAny<string>()), Times.Once);

            IndexProvider
              .Verify(o => o.DeleteIndexes(IndexToRetain, It.IsAny<string>()), Times.Once);

            ElasticClient
              .Verify(x => x.CreateIndex(IndexName, It.IsAny<Func<CreateIndexDescriptor, ICreateIndexRequest>>(), string.Empty), Times.Once);
        }

        [Test]
        public async Task ShouldCallDeleteIndexWhenDownloadFails()
        {
            IndexNameCreator
                .Setup(o => o.CreateNewIndexName(IndexName, SearchCategory.Account))
                .Returns(IndexName);

            Downloader
               .Setup(o => o.LastCode)
               .Returns(HttpStatusCode.InternalServerError);

            var sut = new AccountIndexResourceProcessor(Downloader.Object,
                                                        IndexProvider.Object,
                                                        SearchSettings.Object,
                                                        Logger.Object,
                                                        IndexNameCreator.Object,
                                                        ElasticClient.Object);

            await sut.ProcessResource(new IndexResourceProcessorModel
            {
                BasUri = BaseUrl,
                SiteResource = AccountSiteResource,
                ResourceIdentifier = ResourceIdentifier
            });

            IndexProvider.Verify(o => o.DeleteIndex(IndexName), Times.Once);
        }
    }
}