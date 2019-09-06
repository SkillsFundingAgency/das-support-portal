using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using Elasticsearch.Net;
using FluentAssertions;
using Moq;
using Nest;
using NUnit.Framework;
using SFA.DAS.NLog.Logger;
using SFA.DAS.Support.Common.Infrastucture.Elasticsearch;
using SFA.DAS.Support.Common.Infrastucture.Settings;
using SFA.DAS.Support.Shared.SearchIndexModel;

namespace SFA.DAS.Support.Common.Infrastucture.UnitTests
{
    [TestFixture]
    public class ElasticSearchIndexProviderTest
    {
        [SetUp]
        public void Setup()
        {
            _loggerMock = new Mock<ILog>();
            _clientMock = new Mock<IElasticsearchCustomClient>();
            _settings = new Mock<ISearchSettings>();

            _settings
                .Setup(x => x.IndexShards)
                .Returns(1);

            _settings
                .Setup(x => x.IndexReplicas)
                .Returns(0);

            _loggerMock
                .Setup(x => x.Error(It.IsAny<Exception>(), It.IsAny<string>()));
        }

        private Mock<IElasticsearchCustomClient> _clientMock;
        private Mock<ILog> _loggerMock;
        private Mock<ISearchSettings> _settings;

        private ElasticSearchIndexProvider _sut;

        private const string _indexName = "DummyIndex";
        private const string _aliasName = "DummyAlias";

        [Test]
        public void ShouldCallClientCreateIndexAliasIfAliasDoNotExist()
        {
            //Arrange 
            _clientMock
                .Setup(x => x.AliasExists(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(false)
                .Verifiable();

            _clientMock
                .Setup(x => x.Alias(_aliasName, _indexName, string.Empty))
                .Verifiable();

            //Act
            _sut = new ElasticSearchIndexProvider(_clientMock.Object, _loggerMock.Object, _settings.Object);
            _sut.CreateIndexAlias(_indexName, _aliasName);

            //Assert 
            _clientMock
                .Verify(x => x.AliasExists(It.IsAny<string>(), It.IsAny<string>()),
                    Times.AtLeastOnce);

            _clientMock
                .Verify(x => x.Alias(_aliasName, _indexName, string.Empty), Times.AtLeastOnce);
        }


        [Test]
        public void ShouldCallClientWhenCheckingIfIndexExists()
        {
            //Arrange 
            _clientMock
                .Setup(x => x.IndexExists(_indexName, string.Empty))
                .Returns(true)
                .Verifiable();

            //Act
            _sut = new ElasticSearchIndexProvider(_clientMock.Object, _loggerMock.Object, _settings.Object);
            _sut.IndexExists(_indexName);

            //Assert 
            _clientMock
                .Verify(x => x.IndexExists(_indexName, string.Empty), Times.AtLeastOnce);
        }

        [Test]
        public void ShouldCallClientWhenCreatingIndex()
        {
            //Arrange 
            _clientMock
                .Setup(x => x.IndexExists(_indexName, string.Empty))
                .Returns(false);

            _clientMock
                .Setup(x => x.CreateIndex(_indexName, It.IsAny<Func<CreateIndexDescriptor, ICreateIndexRequest>>(),
                    string.Empty))
                .Returns(new Elasticsearch.CreateIndexResponse { HttpStatusCode = (int)HttpStatusCode.OK });


            //Act
            _sut = new ElasticSearchIndexProvider(_clientMock.Object, _loggerMock.Object, _settings.Object);
            _sut.CreateIndex<UserSearchModel>(_indexName);

            //Assert 
            _clientMock
                .Verify(x => x.IndexExists(_indexName, string.Empty), Times.AtLeastOnce);

            _clientMock
                .Verify(
                    x => x.CreateIndex(_indexName, It.IsAny<Func<CreateIndexDescriptor, ICreateIndexRequest>>(),
                        string.Empty), Times.AtLeastOnce);
        }


        [Test]
        public void ShouldCallClientWhenDeletingIndex()
        {
            //Arrange 
            var deleteResponse = new Elasticsearch.DeleteIndexResponse { Acknowledged = true };
            _clientMock
                .Setup(x => x.DeleteIndex(_indexName, string.Empty))
                .Returns(deleteResponse);

            //Act
            _sut = new ElasticSearchIndexProvider(_clientMock.Object, _loggerMock.Object, _settings.Object);
            _sut.DeleteIndex(_indexName);

            //Assert 
            _clientMock
                .Verify(x => x.DeleteIndex(_indexName, string.Empty), Times.AtLeastOnce);
        }

        [Test]
        public void ShouldCallClientWhenIndexingDocuments()
        {
            //Arrange 

            var documents = new List<UserSearchModel>
            {
                new UserSearchModel
                {
                    Id = "A001"
                }
            };

            _clientMock
                .Setup(x => x.BulkAll(documents, _indexName, 1000))
                .Verifiable();


            //Act
            _sut = new ElasticSearchIndexProvider(_clientMock.Object, _loggerMock.Object, _settings.Object);
            _sut.IndexDocuments(_indexName, documents);

            //Assert 
            _clientMock
                .Verify(x => x.BulkAll(documents, _indexName, 1000), Times.AtLeastOnce);
        }

        [Test]
        public void ShouldDeleteOnlyOldIndexes()
        {
            var indexToDelete01 = "at-das-support-portal-account_20180119100000";
            var indexToDelete02 = "at-das-support-portal-account_20180119103000";
            var indexToKeep01 = "at-das-support-portal-account_20180119110000";
            var indexToKeep02 = "at-das-support-portal-account_20180119113000";
            //Arrange 
            var indexList = new Dictionary<string, IndicesStats>();
            indexList.Add(indexToDelete01, new IndicesStats());
            indexList.Add(indexToKeep01, new IndicesStats());
            indexList.Add(indexToDelete02, new IndicesStats());
            indexList.Add(indexToKeep02, new IndicesStats());

            var readOnlyMockResult = new ReadOnlyDictionary<string, IndicesStats>(indexList);

            var indicesStatsResult = new Elasticsearch.IndicesStatsResponse { Indices = readOnlyMockResult };

            _clientMock
                .Setup(x => x.IndicesStats(Indices.All, null, It.IsAny<string>()))
                .Returns(indicesStatsResult)
                .Verifiable();

            var deleteResponse = new Elasticsearch.DeleteIndexResponse { Acknowledged = true };
            _clientMock
                .Setup(x => x.DeleteIndex(It.IsAny<IndexName>(), It.IsAny<string>()))
                .Returns(deleteResponse)
                .Verifiable();

            //Act
            _sut = new ElasticSearchIndexProvider(_clientMock.Object, _loggerMock.Object, _settings.Object);
            _sut.DeleteIndexes(2, "at-das-support-portal-account");

            ////Assert 

            _clientMock
                .Verify(x => x.DeleteIndex(indexToDelete01, It.IsAny<string>()), Times.Once);

            _clientMock
                .Verify(x => x.DeleteIndex(indexToDelete02, It.IsAny<string>()), Times.Once);

            _clientMock
                .Verify(x => x.DeleteIndex(It.IsAny<IndexName>(), It.IsAny<string>()), Times.Exactly(2));
        }

        [Test]
        public void ShouldSwapAliasIfAliasAlreadyExist()
        {
            //Arrange 
            _clientMock
                .Setup(x => x.AliasExists(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(true)
                .Verifiable();

            _clientMock
                .Setup(x => x.GetIndicesPointingToAlias(_aliasName, string.Empty))
                .Returns(new List<string> {_indexName})
                .Verifiable();

            _clientMock
                .Setup(x => x.Alias(It.IsAny<IBulkAliasRequest>(), string.Empty))
                .Verifiable();

            //Act
            _sut = new ElasticSearchIndexProvider(_clientMock.Object, _loggerMock.Object, _settings.Object);
            _sut.CreateIndexAlias(_indexName, _aliasName);

            //Assert 
            _clientMock
                .Verify(x => x.AliasExists(It.IsAny<string>(), It.IsAny<string>()),
                    Times.AtLeastOnce);

            _clientMock
                .Verify(x => x.Alias(It.IsAny<IBulkAliasRequest>(), string.Empty), Times.AtLeastOnce);

            _clientMock
                .Verify(x => x.GetIndicesPointingToAlias(_aliasName, string.Empty), Times.AtLeastOnce);
        }

        [Test]
        public void ShouldThrowExceptiontWhenIndexDeletionRespnseIsInValid()
        {
            //Arrange 
            Elasticsearch.DeleteIndexResponse deleteResponse = null;

            _clientMock
                .Setup(x => x.DeleteIndex(_indexName, string.Empty))
                .Returns(deleteResponse);

            //Act
            _sut = new ElasticSearchIndexProvider(_clientMock.Object, _loggerMock.Object, _settings.Object);
            Action action = () => _sut.DeleteIndex(_indexName);

            //Assert 
            action.ShouldThrow<Exception>();
        }

        [Test]
        public void ShouldThrowExceptionWhenIndexCreationStatusIsNotOk()
        {
            //Arrange 
            _clientMock
                .Setup(x => x.CreateIndex(_indexName, It.IsAny<Func<CreateIndexDescriptor, ICreateIndexRequest>>(),
                    string.Empty))
                .Returns(new Elasticsearch.CreateIndexResponse { HttpStatusCode = (int)HttpStatusCode.BadRequest })
                .Verifiable();

            //Act
            _sut = new ElasticSearchIndexProvider(_clientMock.Object, _loggerMock.Object, _settings.Object);
            Action action = () => _sut.CreateIndex<UserSearchModel>(_indexName);

            //Assert 
            action.ShouldThrow<Exception>();
        }
    }
}