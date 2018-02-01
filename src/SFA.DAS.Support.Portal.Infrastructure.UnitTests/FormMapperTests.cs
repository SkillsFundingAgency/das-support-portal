using NUnit.Framework;
using SFA.DAS.Support.Portal.Infrastructure.Services;

namespace SFA.DAS.Support.Portal.Infrastructure.UnitTests
{
    [TestFixture]
    public class FormMapperTests
    {
        [SetUp]
        public void Setup()
        {
            _unit = new FormMapper();
        }

        private FormMapper _unit;
        [Test]
        public void ItShouldStripTheUniqueHtmlTagsFromTheFormHtml()
        {
            var id = "123";
            var key = "123123";
            var url = $"/api/challenge/response/{id}";
            var html = "<html><head></head><body><form action='' method='post'></form></body></html>";

            var exepctedHtml =
                $"<form action=\"/resource/challenge?resourceId={id}&amp;key={key}\" method=\"post\"><input name=\"challengeKey\" value=\"{key}\" type=\"hidden\"><input name=\"innerAction\" value=\"\" type=\"hidden\"><input name=\"redirect\" value=\"/api/challenge/response/{id}\" type=\"hidden\"></form>";

            var actual = _unit.UpdateForm(key, id, url, html);

            Assert.AreEqual(exepctedHtml, actual);
            Assert.IsTrue(actual.Contains(url));
            Assert.IsTrue(actual.Contains("challengeKey"));
            Assert.IsTrue(actual.Contains("innerAction"));
            Assert.IsTrue(actual.Contains("redirect"));
        }

        [Test]
        public void ItShouldMapTheForm()
        {
            var id = "123";
            var key = "123123";
            var url = $"/api/challenge/response/{id}";
            var html = "<html><head></head><body><form action='' method='post'></form></body></html>";

            var exepctedHtml =
                $"<form action=\"/resource/challenge?resourceId={id}&amp;key={key}\" method=\"post\"><input name=\"challengeKey\" value=\"{key}\" type=\"hidden\"><input name=\"innerAction\" value=\"\" type=\"hidden\"><input name=\"redirect\" value=\"/api/challenge/response/{id}\" type=\"hidden\"></form>";

            var actual = _unit.UpdateForm(key, id, url, html);

            Assert.AreEqual(exepctedHtml, actual);
            Assert.IsTrue(actual.Contains(url));
            Assert.IsTrue(actual.Contains("challengeKey"));
            Assert.IsTrue(actual.Contains("innerAction"));
            Assert.IsTrue(actual.Contains("redirect"));
        }
    }
}