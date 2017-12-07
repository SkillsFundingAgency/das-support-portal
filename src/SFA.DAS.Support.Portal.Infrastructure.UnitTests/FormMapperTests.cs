using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using SFA.DAS.Support.Portal.Infrastructure.Services;

namespace SFA.DAS.Support.Portal.Infrastructure.UnitTests
{
    [TestFixture]
    public class FormMapperTests
    {
        private FormMapper _unit;

        [SetUp]
        public void Setup()
        {
            _unit = new FormMapper();
        }

        [Test]
        public void ItShouldMapTheForm()
        {
            var html = "<html><head></head><body><form action='post'></form></body></html>";
            var exepctedHtml = "<html><head></head><body><form action=\"/resource/challenge?resourceId=id&amp;key=key\"><input name=\"challengeKey\" value=\"key\" type=\"hidden\"><input name=\"innerAction\" value=\"post\" type=\"hidden\"><input name=\"redirect\" value=\"url\" type=\"hidden\"></form></body></html>";
            var actual = _unit.UpdateForm("key", "id", "url", html);

            Assert.AreEqual(exepctedHtml, actual);
        }
    }
}
