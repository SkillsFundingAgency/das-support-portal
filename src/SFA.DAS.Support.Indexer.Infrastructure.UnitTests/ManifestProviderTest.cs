using NUnit.Framework;
using SFA.DAS.Support.Indexer.Infrastructure.Manifest;
using SFA.DAS.Support.Shared.SearchIndexModel;
using SFA.DAS.Support.Shared.SiteConnection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFA.DAS.Support.Indexer.Infrastructure.UnitTests
{
    [TestFixture]
    public class ManifestProviderTest
    {

        [Test]
        public void  ShouldThrowExceptionForNon200Response()
        {
            var mockSiteConnector = new Moq.Mock<ISiteConnector>();
            var accountSearchModels = new List<AccountSearchModel>();
            var url = new Uri("http://localhost");

            mockSiteConnector
                .Setup(o => o.Download<IEnumerable<AccountSearchModel>>(url))
                .Returns(Task.FromResult(accountSearchModels.AsEnumerable()));

            mockSiteConnector
                .Setup(o => o.LastCode)
                .Returns(System.Net.HttpStatusCode.InternalServerError);

            var _sut = new ManifestProvider(mockSiteConnector.Object);

           Assert.ThrowsAsync<Exception>(async () => await _sut.GetSearchItems<IEnumerable<AccountSearchModel>>(url));
        }

    }
}
