using System;
using FluentAssertions;
using Moq;
using Nest;
using NUnit.Framework;
using SFA.DAS.Support.Common.Infrastucture.Elasticsearch;
using SFA.DAS.Support.Common.Infrastucture.Settings;

namespace SFA.DAS.Support.Common.Infrastucture.UnitTests
{
    [TestFixture]
    public class ElasticsearchClientFactoryTest
    {
        [SetUp]
        public void Setup()
        {
            _settingMock = new Mock<ISearchSettings>();
            _settingMock.Setup(x => x.IgnoreSslCertificateEnabled).Returns(true);
            _settingMock.Setup(x => x.ServerUrls).Returns("https://localhost:9200/");
            _settingMock.Setup(x => x.ElasticServerUrls).Returns(new[] {new Uri("https://localhost:9200/")});
            _settingMock.Setup(x => x.Elk5Enabled).Returns(true);
            _sut = new ElasticsearchClientFactory(_settingMock.Object);
        }

        private ElasticsearchClientFactory _sut;
        private Mock<ISearchSettings> _settingMock;

        [Test]
        public void ShouldReturnElasticSearchInstance()
        {
            var result = _sut.GetElasticClient();
            result.Should().BeOfType<ElasticClient>();

            result = null;
        }
    }
}