using NUnit.Framework;
using SFA.DAS.NLog.Logger;
using SFA.DAS.Support.Portal.Infrastructure.Services;
using SFA.DAS.Support.Shared.Discovery;

namespace SFA.DAS.Support.Portal.Infrastructure.UnitTests
{
    [TestFixture]
    public class FormMapperTests
    {
        [SetUp]
        public void Setup()
        {
            _unit = new FormMapper(Moq.Mock.Of<ILog>());
        }

        private FormMapper _unit;

        [Test]
        public void ItShouldMapTheForm()
        {
            var id = "123";
            var challengeKey = SupportServiceResourceKey.EmployerAccountFinanceChallenge;
            var resourceKey = SupportServiceResourceKey.EmployerAccountFinance;
            var url = $"/api/challenge/response/{id}";
            var html = "<html><head></head><body><form action='' method='post'></form></body></html>";

            var exepctedHtml =
                $"<form action=\"/resource/challenge?resourceId={id}&amp;resourceKey={resourceKey}&amp;challengeKey={challengeKey}\" method=\"post\"><input name=\"resourceKey\" value=\"{resourceKey}\" type=\"hidden\"><input name=\"challengeKey\" value=\"{challengeKey}\" type=\"hidden\"><input name=\"innerAction\" value=\"\" type=\"hidden\"><input name=\"redirect\" value=\"/api/challenge/response/{id}\" type=\"hidden\"></form>";

            var actual = _unit.UpdateForm(resourceKey, challengeKey, id, url, html);

            Assert.AreEqual(exepctedHtml, actual);
            Assert.IsTrue(actual.Contains(url));
            Assert.IsTrue(actual.Contains("resourceKey"));
            Assert.IsTrue(actual.Contains("challengeKey"));
            Assert.IsTrue(actual.Contains("innerAction"));
            Assert.IsTrue(actual.Contains("redirect"));
        }

        [Test]
        public void ItShouldStripTheUniqueHtmlTagsFromTheFormHtml()
        {
            var id = "123";
            var challengeKey = SupportServiceResourceKey.EmployerAccountFinanceChallenge;
            var resourceKey = SupportServiceResourceKey.EmployerAccountFinance;
            var url = $"/api/challenge/response/{id}";
            var html = "<html><head></head><body><form action='' method='post'></form></body></html>";

            var exepctedHtml =
                $"<form action=\"/resource/challenge?resourceId={id}&amp;resourceKey={resourceKey}&amp;challengeKey={challengeKey}\" method=\"post\"><input name=\"resourceKey\" value=\"{resourceKey}\" type=\"hidden\"><input name=\"challengeKey\" value=\"{challengeKey}\" type=\"hidden\"><input name=\"innerAction\" value=\"\" type=\"hidden\"><input name=\"redirect\" value=\"/api/challenge/response/{id}\" type=\"hidden\"></form>";

            var actual = _unit.UpdateForm(resourceKey, challengeKey, id, url, html);

            Assert.AreEqual(exepctedHtml, actual);
            Assert.IsTrue(actual.Contains(url));
            Assert.IsTrue(actual.Contains("resourceKey"));
            Assert.IsTrue(actual.Contains("challengeKey"));
            Assert.IsTrue(actual.Contains("innerAction"));
            Assert.IsTrue(actual.Contains("redirect"));
        }
    }
}