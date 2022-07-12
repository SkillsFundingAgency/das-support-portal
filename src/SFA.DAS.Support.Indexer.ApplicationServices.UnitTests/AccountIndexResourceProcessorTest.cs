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
            _indexNameCreator
                .Setup(o => o.CreateNewIndexName(_indexName, SearchCategory.User))
                .Returns(_indexName);

            var sut = new AccountIndexResourceProcessor(_siteSettings.Object,
                                                        _downloader.Object,
                                                        _indexProvider.Object,
                                                        _searchSettings.Object,
                                                        _logger.Object,
                                                        _indexNameCreator.Object,
                                                        _elasticClient.Object);

            await sut.ProcessResource(new IndexResourceProcessorModel
            {
                BasUri = _baseUrl,
                SiteResource = _userSiteResource,
                ResourceIdentifier = _resourceIdentifier
            });

            _indexNameCreator
                .Verify(o => o.CreateNewIndexName(_indexName, SearchCategory.User), Times.Never);
        }

        [Test]
        public async Task ShouldNotProcessAccountModelIfUnauthorised()
        {
            _downloader
                .Setup(o => o.Download<IEnumerable<AccountSearchModel>>(_baseUrl, It.IsAny<string>()))
                .Returns(Task.FromResult(_accountModels));

            _downloader
                .Setup(o => o.Download(It.IsAny<Uri>(), It.IsAny<string>()))
                .Returns(Task.FromResult(null as string));

            _downloader
                .Setup(o => o.LastCode)
                .Returns(HttpStatusCode.Unauthorized);

            var sut = new AccountIndexResourceProcessor(_siteSettings.Object,
                _downloader.Object,
                _indexProvider.Object,
                _searchSettings.Object,
                _logger.Object,
                _indexNameCreator.Object,
                _elasticClient.Object);

            await sut.ProcessResource(new IndexResourceProcessorModel
            {
                BasUri = _baseUrl,
                SiteResource = _accountSiteResource,
                ResourceIdentifier = _resourceIdentifier
            });

            _indexNameCreator
                .Verify(o => o.CreateNewIndexName(_indexName, SearchCategory.Account), Times.Once);

            _indexNameCreator
                .Verify(o => o.CreateIndexesAliasName(_indexName, SearchCategory.Account), Times.Never);

            _elasticClient
                .Verify(o => o.IndexExists(_indexName, string.Empty), Times.Once);

            _downloader
                .Verify(o => o.Download(It.IsAny<Uri>(), It.IsAny<string>()), Times.Exactly(3));

            _indexProvider
                .Verify(o => o.DeleteIndex(_indexName), Times.Once);

            _indexProvider
                .Verify(o => o.CreateIndexAlias(_indexName, It.IsAny<string>()), Times.Never);

            _indexProvider
                .Verify(o => o.DeleteIndexes(_indexToRetain, It.IsAny<string>()), Times.Never);

            _elasticClient
                .Verify(x => x.CreateIndex(_indexName, It.IsAny<Func<CreateIndexDescriptor, ICreateIndexRequest>>(), string.Empty), Times.Once);
        }

        [Test]
        public async Task ShouldProcessAccountModel()
        {
            var sut = new AccountIndexResourceProcessor(_siteSettings.Object,
                                                        _downloader.Object,
                                                        _indexProvider.Object,
                                                        _searchSettings.Object,
                                                        _logger.Object,
                                                        _indexNameCreator.Object,
                                                        _elasticClient.Object);

            await sut.ProcessResource(new IndexResourceProcessorModel
            {
                BasUri = _baseUrl,
                SiteResource = _accountSiteResource,
                ResourceIdentifier = _resourceIdentifier
            });

            _indexNameCreator
                .Verify(o => o.CreateNewIndexName(_indexName, SearchCategory.Account), Times.Once);

            _indexNameCreator
                .Verify(o => o.CreateIndexesAliasName(_indexName, SearchCategory.Account), Times.Once);

            _elasticClient
                .Verify(o => o.IndexExists(_indexName, string.Empty), Times.Once);

            _downloader
                .Verify(o => o.Download<IEnumerable<AccountSearchModel>>(It.IsAny<Uri>(), It.IsAny<string>()), Times.AtLeastOnce);

            _indexProvider
                .Verify(o => o.DeleteIndex(_indexName), Times.Never);

            _indexProvider
               .Verify(o => o.CreateIndexAlias(_indexName, It.IsAny<string>()), Times.Once);

            _indexProvider
              .Verify(o => o.DeleteIndexes(_indexToRetain, It.IsAny<string>()), Times.Once);

            _elasticClient
              .Verify(x => x.CreateIndex(_indexName, It.IsAny<Func<CreateIndexDescriptor, ICreateIndexRequest>>(), string.Empty), Times.Once);
        }

        [Test]
        public async Task ShouldCallDeleteIndexWhenDownloadFails()
        {
            _indexNameCreator
                .Setup(o => o.CreateNewIndexName(_indexName, SearchCategory.Account))
                .Returns(_indexName);

            _downloader
               .Setup(o => o.LastCode)
               .Returns(HttpStatusCode.InternalServerError);

            var sut = new AccountIndexResourceProcessor(_siteSettings.Object,
                                                        _downloader.Object,
                                                        _indexProvider.Object,
                                                        _searchSettings.Object,
                                                        _logger.Object,
                                                        _indexNameCreator.Object,
                                                        _elasticClient.Object);

            await sut.ProcessResource(new IndexResourceProcessorModel
            {
                BasUri = _baseUrl,
                SiteResource = _accountSiteResource,
                ResourceIdentifier = _resourceIdentifier
            });

            _indexProvider.Verify(o => o.DeleteIndex(_indexName), Times.Once);
        }
    }
}