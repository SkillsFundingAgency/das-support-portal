using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.Support.Shared.Discovery;

namespace SFA.DAS.Support.Portal.ApplicationServices.UnitTests.Services.ManifestRepository
{
    [TestFixture]
    public class WhenCallingGetChallengeForm : WhenTestingManifestRepository
    {
       
        [Test]
        public void ItShouldNotThrowAnExceptionIfTheSiteManifestNull()
        {
            MockSiteConnector.Setup(x => x.Download<SiteManifest>(TestSiteUri)).ReturnsAsync(null as SiteManifest);

            Assert.DoesNotThrow(() =>
            {
                var response = Unit.GetChallengeForm(SupportServiceResourceKey.EmployerAccountFinance, SupportServiceResourceKey.EmployerAccountFinanceChallenge, "id",
                    "http://tempuri.org/callenge/form");
            });
        }

        [Test]
        public async Task ItShouldReturnTheChallengeForm()
        {
            var downloadedFormHtml = "<html><form action='' method='post' /></html>";
            var mappedFormHtml = "<html><form action='/api/challenge/id'  method='post' /></html>";


            MockSiteConnector.Setup(c => c.Download(It.IsAny<string>()))
                .ReturnsAsync(downloadedFormHtml);

            MockFormMapper.Setup(x => x.UpdateForm(
                    It.IsAny<SupportServiceResourceKey>(),
                    It.IsAny<SupportServiceResourceKey>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()
                ))
                .Returns(mappedFormHtml);

            var actual = await Unit.GetChallengeForm(
                SupportServiceResourceKey.EmployerAccountFinance,
                SupportServiceResourceKey.EmployerAccountFinanceChallenge, 
                "id",
                "http://tempuri.org/challenge/form");

            Assert.IsFalse(string.IsNullOrWhiteSpace(actual));
            Assert.IsTrue(actual.Contains("<html"));
            Assert.IsTrue(actual.Contains("<form"));
        }

       
    }
}