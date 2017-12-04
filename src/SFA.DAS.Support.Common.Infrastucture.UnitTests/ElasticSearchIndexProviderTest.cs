using Elasticsearch.Net;
using FluentAssertions;
using Moq;
using Nest;
using NUnit.Framework;
using SFA.DAS.NLog.Logger;
using SFA.DAS.Support.Common.Infrastucture.Elasticsearch;
using SFA.DAS.Support.Common.Infrastucture.Settings;
using SFA.DAS.Support.Shared;
using System;
using System.Collections.Generic;
using System.Net;


namespace SFA.DAS.Support.Common.Infrastucture.UnitTests
{
    [TestFixture]
    public class ElasticSearchIndexProviderTest
    {

        private Mock<IElasticsearchCustomClient> _clientMock;
        private Mock<ILog> _loggerMock;
        private  Mock<ISearchSettings> _settings;

        private ElasticSearchIndexProvider _sut;

        private const string _indexName = "DummyIndex";
        private const string _aliasName = "DummyAlias";

        [SetUp]
        public void Satup()
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

        }

        [Test]
        public void ShouldCallClientWhenCreatingIndex()
        {
            //Arrange 
            var apiCall = new Mock<IApiCallDetails>();
            apiCall.Setup(x => x.HttpStatusCode)
                .Returns((int)HttpStatusCode.OK);

            var response = new Mock<ICreateIndexResponse>();
            response
                .Setup(o => o.ApiCall)
                .Returns(apiCall.Object);

            _clientMock
             .Setup(x => x.CreateIndex(_indexName, It.IsAny<Func<CreateIndexDescriptor, ICreateIndexRequest>>(), string.Empty))
             .Returns(response.Object);

            //Act
            _sut = new ElasticSearchIndexProvider(_clientMock.Object, _loggerMock.Object, _settings.Object);
            _sut.CreateIndex<SearchItem>(_indexName);

            //Assert 
            _clientMock
             .Verify(x => x.CreateIndex(_indexName, It.IsAny<Func<CreateIndexDescriptor, ICreateIndexRequest>>(), string.Empty), Times.AtLeastOnce);
        }

        [Test]
        public void ShouldThrowExceptionWhenIndexCreationStatusIsNotOk()
        {
            //Arrange 

            var response = new Mock<ICreateIndexResponse>();
            response
                .Setup(o => o.ApiCall.HttpStatusCode)
                .Returns((int)HttpStatusCode.BadRequest);


            _clientMock
             .Setup(x => x.CreateIndex(_indexName, It.IsAny<Func<CreateIndexDescriptor, ICreateIndexRequest>>(), string.Empty))
             .Returns(response.Object)
             .Verifiable();

            //Act
             _sut = new ElasticSearchIndexProvider(_clientMock.Object, _loggerMock.Object, _settings.Object);
            Action action = () => _sut.CreateIndex<SearchItem>(_indexName);

            //Assert 
            action.ShouldThrow<Exception>();

        }

        [Test]
        public void ShouldCallClientWhenIndexingDocuments()
        {
            //Arrange 

            var documents = new List<SearchItem>
            {
                new SearchItem
                {
                    SearchId = "A001",
                    Html = "<div></div>"
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
        public void ShouldCallClientWhenDeletingIndex()
        {
            //Arrange 
            var deleteResponse = new Mock<IDeleteIndexResponse>();
            deleteResponse
                .Setup(x => x.Acknowledged)
                .Returns(true);

            _clientMock
             .Setup(x => x.DeleteIndex(_indexName, string.Empty))
            .Returns(deleteResponse.Object);

            //Act
             _sut = new ElasticSearchIndexProvider(_clientMock.Object, _loggerMock.Object, _settings.Object);
            _sut.DeleteIndex(_indexName);

            //Assert 
            _clientMock
               .Verify(x => x.DeleteIndex(_indexName, string.Empty), Times.AtLeastOnce);

        }

        [Test]
        public void ShouldThrowExceptiontWhenIndexDeletionRespnseIsInValid()
        {
            //Arrange 
            IDeleteIndexResponse deleteResponse = null;

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
        public void ShouldCallClientWhenCheckingIfIndexExists()
        {
            //Arrange 
            var indexExistsResponse = new Mock<IExistsResponse>();
            indexExistsResponse
                .Setup(x => x.Exists)
                .Returns(true);

            _clientMock
             .Setup(x => x.IndexExists(_indexName, string.Empty))
             .Returns(indexExistsResponse.Object)
             .Verifiable();

            //Act
             _sut = new ElasticSearchIndexProvider(_clientMock.Object, _loggerMock.Object, _settings.Object);
            _sut.IndexExists(_indexName);

            //Assert 
            _clientMock
            .Verify(x => x.IndexExists(_indexName, string.Empty), Times.AtLeastOnce);

        }

        [Test]
        public void ShouldCallClientCreateIndexAliasIfAliasDoNotExist()
        {
            //Arrange 
            var aliasExist = false;

            var objectExistsResponse = new Mock<IExistsResponse>();
            objectExistsResponse
                .Setup(x => x.Exists)
                .Returns(aliasExist);

            _clientMock
             .Setup(x => x.AliasExists(It.IsAny<Func<AliasExistsDescriptor, IAliasExistsRequest>>(), string.Empty))
             .Returns(objectExistsResponse.Object)
             .Verifiable();

            _clientMock
             .Setup(x => x.Alias(_aliasName, _indexName, string.Empty))
             .Verifiable();

            //Act
             _sut = new ElasticSearchIndexProvider(_clientMock.Object, _loggerMock.Object, _settings.Object);
            _sut.CreateIndexAlias(_indexName, _aliasName);

            //Assert 
            _clientMock
             .Verify(x => x.AliasExists(It.IsAny<Func<AliasExistsDescriptor, IAliasExistsRequest>>(), string.Empty), Times.AtLeastOnce);

            _clientMock
            .Verify(x => x.Alias(_aliasName, _indexName, string.Empty), Times.AtLeastOnce);
        }

        [Test]
        public void ShouldSwapAliasIfAliasAlreadyExist()
        {
            //Arrange 
            var aliasExist = true;

            var objectExistsResponse = new Mock<IExistsResponse>();
            objectExistsResponse
                .Setup(x => x.Exists)
                .Returns(aliasExist);

            _clientMock
             .Setup(x => x.AliasExists(It.IsAny<Func<AliasExistsDescriptor, IAliasExistsRequest>>(), string.Empty))
             .Returns(objectExistsResponse.Object)
             .Verifiable();

            _clientMock
             .Setup(x => x.GetIndicesPointingToAlias(_aliasName, string.Empty))
             .Returns(new List<string> { _indexName })
             .Verifiable();

            _clientMock
             .Setup(x => x.Alias(It.IsAny<IBulkAliasRequest>(), string.Empty))
             .Verifiable();

            //Act
            _sut = new ElasticSearchIndexProvider(_clientMock.Object, _loggerMock.Object, _settings.Object);
            _sut.CreateIndexAlias(_indexName, _aliasName);

            //Assert 
            _clientMock
             .Verify(x => x.AliasExists(It.IsAny<Func<AliasExistsDescriptor, IAliasExistsRequest>>(), string.Empty), Times.AtLeastOnce);

            _clientMock
            .Verify(x => x.Alias(It.IsAny<IBulkAliasRequest>(), string.Empty), Times.AtLeastOnce);

            _clientMock
            .Verify(x => x.GetIndicesPointingToAlias(_aliasName, string.Empty), Times.AtLeastOnce);

        }


    }
}
