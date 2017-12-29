using Elasticsearch.Net;
using FluentAssertions;
using Moq;
using Nest;
using NUnit.Framework;
using SFA.DAS.Support.Common.Infrastucture.Elasticsearch;
using SFA.DAS.Support.Shared.SearchIndexModel;
using System;
using System.Collections.Generic;
using System.Net;

namespace SFA.DAS.Support.Common.Infrastucture.UnitTests
{
    [TestFixture]
    public class ElasticSearchProviderTest
    {

        private Mock<IElasticsearchCustomClient> _clientMock;
        private const string _indexAliasName = "DummyIndex";
        private ElasticSearchProvider _sut;

        [SetUp]
        public void Setup()
        {
            _clientMock = new Mock<IElasticsearchCustomClient>();
        }

        [Test]
        public void ShouldReturnOnlyUserSearchDocuments()
        {
            //Arrange
            var documents = new List<UserSearchModel>
            {
                new UserSearchModel
                {
                    Id = "A001"
                }
           };

            var apiCall = new Mock<IApiCallDetails>();

            apiCall
                .Setup(x => x.HttpStatusCode)
                .Returns((int)HttpStatusCode.OK);

            var response = new Mock<ISearchResponse<UserSearchModel>>();
            response.Setup(x => x.Documents).Returns(documents);
            response.Setup(x => x.ApiCall).Returns(apiCall.Object);
            _clientMock
                .Setup(x => x.Search(It.IsAny<Func<SearchDescriptor<UserSearchModel>, ISearchRequest>>(), string.Empty))
                .Returns(response.Object);

            var countResponse = new Mock<ICountResponse>();
            countResponse.Setup(x => x.ApiCall).Returns(apiCall.Object);
            countResponse.Setup(x => x.Count).Returns(documents.Count);
            _clientMock
                .Setup(x => x.Count(It.IsAny<Func<CountDescriptor<UserSearchModel>, ICountRequest>>(),string.Empty))
                .Returns(countResponse.Object);
            
            _sut = new ElasticSearchProvider(_clientMock.Object, _indexAliasName);
            //Act
            var result = _sut.FindUsers("A001",SearchCategory.User);

            //Assert
            result.Should().NotBeNull();
            result.TotalCount.Should().Be(documents.Count);
            result.Results.Should().AllBeOfType<UserSearchModel>();
        }

    }
}
