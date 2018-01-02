using Elasticsearch.Net;
using FluentAssertions;
using Moq;
using Nest;
using NUnit.Framework;
using SFA.DAS.Support.Common.Infrastucture.Elasticsearch;
using SFA.DAS.Support.Common.Infrastucture.Indexer;
using SFA.DAS.Support.Common.Infrastucture.Settings;
using SFA.DAS.Support.Portal.Core.Configuration;
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
        private Mock<IConfigurationSettings> _configurationSettingsMock;
        private Mock<ISearchSettings> _searchSettingsMock;
        private Mock<IIndexNameCreator> _indexNameCreatorMock;
        private Mock<IApiCallDetails>  _apiCall = new Mock<IApiCallDetails>();

        [SetUp]
        public void Setup()
        {
            _clientMock = new Mock<IElasticsearchCustomClient>();
            _configurationSettingsMock = new Mock<IConfigurationSettings>();
            _searchSettingsMock = new Mock<ISearchSettings>();
            _indexNameCreatorMock = new Mock<IIndexNameCreator>();

            _indexNameCreatorMock
                .Setup(x => x.CreateIndexesAliasName(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<SearchCategory>()))
                .Returns("local_das_test");

            _apiCall
                .Setup(x => x.HttpStatusCode)
                .Returns((int)HttpStatusCode.OK);

        }

        [Test]
        public void ShouldReturnOnlyUserSearchDocuments()
        {
            //Arrange
            var documents = new List<UserSearchModel>
            {
                new UserSearchModel
                {
                    Id = "A001",
                    SearchType = SearchCategory.User
                }
           };

            ConfigureTesShareResources(documents);

            _sut = new ElasticSearchProvider(_clientMock.Object,
                                    _configurationSettingsMock.Object,
                                    _searchSettingsMock.Object,
                                   _indexNameCreatorMock.Object);
            //Act
            var result = _sut.FindUsers("A001", SearchCategory.User);

            //Assert
            result.Should().NotBeNull();
            result.TotalCount.Should().Be(documents.Count);
            result.Results.Should().AllBeOfType<UserSearchModel>();
        }

        [Test]
        public void ShouldReturnOnlyAccountSearchDocuments()
        {
            //Arrange
            var documents = new List<AccountSearchModel>
            {
                new AccountSearchModel
                {
                   Account = "Tesco",
                   AccountID = "B001",
                   SearchType = SearchCategory.Account
                }
           };

            ConfigureTesShareResources(documents);

            _sut = new ElasticSearchProvider(_clientMock.Object,
                                    _configurationSettingsMock.Object,
                                    _searchSettingsMock.Object,
                                   _indexNameCreatorMock.Object);
            //Act
            var result = _sut.FindAccounts("A001", SearchCategory.Account);

            //Assert
            result.Should().NotBeNull();
            result.TotalCount.Should().Be(documents.Count);
            result.Results.Should().AllBeOfType<AccountSearchModel>();
        }


        private void ConfigureTesShareResources<T> (List<T> documents) where T:class
        {
            var response = new Mock<ISearchResponse<T>>();
            response.Setup(x => x.Documents).Returns(documents);
            response.Setup(x => x.ApiCall).Returns(_apiCall.Object);
            _clientMock
                .Setup(x => x.Search(It.IsAny<Func<SearchDescriptor<T>, ISearchRequest>>(), string.Empty))
                .Returns(response.Object);

            var countResponse = new Mock<ICountResponse>();
            countResponse.Setup(x => x.ApiCall).Returns(_apiCall.Object);
            countResponse.Setup(x => x.Count).Returns(documents.Count);

            _clientMock
                .Setup(x => x.Count(It.IsAny<Func<CountDescriptor<T>, ICountRequest>>(), string.Empty))
                .Returns(countResponse.Object);
        }


    }
}
