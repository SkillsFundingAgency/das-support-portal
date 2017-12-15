using Elasticsearch.Net;
using FluentAssertions;
using Moq;
using Nest;
using NUnit.Framework;
using SFA.DAS.Support.Common.Infrastucture.Elasticsearch;
using SFA.DAS.Support.Shared;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

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
        public void ShouldReturnSearchDocuments()
        {
            //Arrange
            var documents = new List<SearchItem>
            {
                new SearchItem
                {
                    SearchId = "A001",
                    SearchResultJson = "{'user':'john'}"
                }
           };

            var apiCall = new Mock<IApiCallDetails>();

            apiCall
                .Setup(x => x.HttpStatusCode)
                .Returns((int)HttpStatusCode.OK);

            var response = new Mock<ISearchResponse<SearchItem>>();
            response.Setup(x => x.Documents).Returns(documents);
            response.Setup(x => x.ApiCall).Returns(apiCall.Object);
            _clientMock
                .Setup(x => x.Search(It.IsAny<Func<SearchDescriptor<SearchItem>, ISearchRequest>>(), string.Empty))
                .Returns(response.Object);

            var countResponse = new Mock<ICountResponse>();
            countResponse.Setup(x => x.ApiCall).Returns(apiCall.Object);
            countResponse.Setup(x => x.Count).Returns(documents.Count);
            _clientMock
                .Setup(x => x.Count(It.IsAny<Func<CountDescriptor<SearchItem>, ICountRequest>>(),string.Empty))
                .Returns(countResponse.Object);
            
            _sut = new ElasticSearchProvider(_clientMock.Object, _indexAliasName);
            //Act
            var result = _sut.Search("A001");

            //Assert
            result.Should().NotBeNull();
            result.TotalCount.Should().Be(documents.Count);
            result.Results.Should().AllBeOfType<SearchItem>();
        }

    }
}
