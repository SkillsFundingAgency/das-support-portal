using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.Support.Shared.Discovery;

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
            var result = await Unit.GetResourcePage(SupportServiceResourceKey.EmployerAccountFinance, "id");
            Assert.IsFalse(string.IsNullOrWhiteSpace(result.Resource));
        }

        [Test]
        public void ItShouldThrowAnExceptionIfTheKeyIsNotFound()
        {
            Assert.ThrowsAsync<KeyNotFoundException>(() =>
                Unit.GetResourcePage(SupportServiceResourceKey.EmployerUserAccountTeam, "id"));
        }

       
    }
}