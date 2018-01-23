using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using Moq;
using NUnit.Framework;

namespace SFA.DAS.Support.Portal.ApplicationServices.UnitTests.Services.ManifestRepository
{
    [TestFixture]
    public class WhenCallingGetResourcePage : WhenTestingManifestRepository
    {
       

        [Test]
        public async Task ItShouldReturnTheHtmlPage()
        {
            var html = "<html>Some page</html>";
            MockSiteConnector.Setup(x => x.Download(It.IsAny<string>()))
                .ReturnsAsync(html);
            var result = await Unit.GetResourcePage("resourcekey", "id");
            Assert.IsFalse(string.IsNullOrWhiteSpace(result.Resource));
        }

        [Test]
        public void ItShouldThrowAnExceptionIfTheKeyIsNotFound()
        {
            Assert.ThrowsAsync<KeyNotFoundException>(() => Unit.GetResourcePage("key", "id"));
        }

        [Test]
        public void ItShouldThrowAnExceptionIfTheSiteBaseUrlIsNull()
        {
            TestSiteManifest.BaseUrl = null;
            Assert.ThrowsAsync<NullReferenceException>(() =>
                Unit.GetResourcePage("resourcekey", "id"));
        }
    }
}