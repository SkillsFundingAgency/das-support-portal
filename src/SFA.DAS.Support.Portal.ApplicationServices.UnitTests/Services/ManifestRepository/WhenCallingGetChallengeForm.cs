using System;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.Support.Shared;

namespace SFA.DAS.Support.Portal.ApplicationServices.UnitTests.Services.ManifestRepository
{
    [TestFixture]
    public class WhenCallingGetChallengeForm : WhenTestingManifestRepository
    {
        [TearDown]
        public void Teardown()
        {
            MockLogger.Verify(x => x.Debug($"Downloading '{TestSiteUri}'"), Times.Once);
        }

        [Test]
        public async Task ItShouldReturnTheChallengeForm()
        {
            var downloadedFormHtml = "<html><form action='' method='post' /></html>";
            var mappedFormHtml = "<html><form action='/api/challenge/id'  method='post' /></html>";

            MockSiteConnector.Setup(c => c.Download(It.IsAny<string>()))
                .ReturnsAsync(downloadedFormHtml);

            MockFormMapper.Setup(x => x.UpdateForm(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()
                ))
                .Returns(mappedFormHtml);

            var actual = await Unit.GetChallengeForm("challengekey", "id", "http://tempuri.org/callenge/form");

            Assert.IsFalse(string.IsNullOrWhiteSpace(actual));

            Assert.IsTrue(actual.Contains("<html"));
            Assert.IsTrue(actual.Contains("<form"));
        }

        [Test]
        public void ItShouldThrowAnExceptionIfTheSiteBaseUrlIsNull()
        {
            TestSiteManifest.BaseUrl = null;
            Assert.ThrowsAsync<NullReferenceException>(() =>
                Unit.GetChallengeForm("challengekey", "id", "http://tempuri.org/callenge/form"));
        }

        [Test]
        public void ItShouldThrowAnExceptionIfTheSiteIsNull()
        {
            MockSiteConnector.Setup(x => x.Download<SiteManifest>(TestSiteUri)).ReturnsAsync(null as SiteManifest);

            Assert.ThrowsAsync<NullReferenceException>(() =>
                Unit.GetChallengeForm("challengekey", "id", "http://tempuri.org/callenge/form"));
        }
    }
}