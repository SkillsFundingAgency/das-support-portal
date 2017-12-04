using NUnit.Framework;
using SFA.DAS.Support.Common.Infrastucture.Elasticsearch;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nest;
using SFA.DAS.Support.Common.Infrastucture.Settings;
using Moq;

namespace SFA.DAS.Support.Common.Infrastucture.UnitTests
{
    [TestFixture]
    public class ElasticsearchClientFactoryTest
    {
        private ElasticsearchClientFactory _sut;
        private Mock<ISearchSettings> _settingMock;

        [SetUp]
        public void Setup()
        {

            _settingMock = new Mock<ISearchSettings>();
            _settingMock.Setup(x => x.IgnoreSslCertificateEnabled).Returns(true);
            _settingMock.Setup(x => x.ElasticServerUrls).Returns(new List<Uri> { new Uri("http://localhost:9200/", UriKind.Absolute) });
            _settingMock.Setup(x => x.Elk5Enabled).Returns(true);
            _sut = new ElasticsearchClientFactory(_settingMock.Object);
        }

        [Test]
        public void ShouldReturnElasticSearchInstance()
        {
            var result = _sut.GetElasticClient();
            result.Should().BeOfType<ElasticClient>();

            result = null;
        }

    }
}
