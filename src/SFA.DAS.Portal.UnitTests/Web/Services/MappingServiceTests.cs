using Moq;
using NUnit.Framework;
using SFA.DAS.NLog.Logger;
using SFA.DAS.Portal.Web.Services;

namespace SFA.DAS.Portal.UnitTests.Web.Services
{
    [TestFixture]
    public sealed class MappingServiceTests
    {
        [Test]
        public void MappingConfigurationShouldBeValid()
        {
            var service = new MappingService(Mock.Of<ILog>());

            service.Configuration.AssertConfigurationIsValid();
        }
    }
}
