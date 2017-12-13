using Moq;
using NUnit.Framework;
using SFA.DAS.NLog.Logger;
using SFA.DAS.Support.Portal.Web.Services;

namespace SFA.DAS.Support.Portal.UnitTests.Web.Services
{
    [TestFixture]
    public sealed class MappingServiceTests
    {
        [Test]
        public void MappingConfigurationShouldBeValid()
        {
            var service = new MappingService(Mock.Of<ILog>(), Mock.Of<ISearchTableResultBuilder>());

            service.Configuration.AssertConfigurationIsValid();
        }
    }
}
