using NUnit.Framework;

namespace SFA.DAS.Support.Portal.UnitTests.Web.Controllers
{
    public class WhenCallingTheVersionControllerGetTestMethod : WithAPreparedVersionController
    {
        [Test]
        public void ItShouldReturnTrue()
        {
            var result = Unit.GetTest();
            Assert.IsTrue(result);
        }
    }
}