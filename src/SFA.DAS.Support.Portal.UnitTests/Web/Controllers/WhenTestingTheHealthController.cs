using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.Support.Portal.Health;
using SFA.DAS.Support.Portal.Web.Controllers;

namespace SFA.DAS.Support.Portal.UnitTests.Web.Controllers
{
    [TestFixture]
    public class WhenTestingTheHealthController : WhenTestingAnMvcControllerOfType<HealthController> 
    {
        private Mock<IHealthService> _healthService;

        [SetUp]
        public override void Setup()
        {
            _healthService = new Mock<IHealthService>();
            Unit = new HealthController(_healthService.Object);
        }
        [Ignore("Unit designed as untestable, refactoring requried to enable testing of this unit")]
        [Test]
        public async Task ItShouldNotThrowAndExceptionWhenCallingTheIndexMethod()
        {
            var healthModel = new Health.Model.HealthModel();
            _healthService.Setup(h => h.CreateHealthModel()).Returns(Task.FromResult(healthModel));
            Assert.DoesNotThrow(async () => ActionResultResponse = await Unit.Index());
            
        }
        [Ignore("Unit designed as untestable, refactoring requried to enable testing of this unit")]
        [Test]
        public async Task ItShouldNotThrowAndExceptionWhenCallingTheAccountsMethod()
        {
            Assert.DoesNotThrow(async () => ActionResultResponse = await Unit.Accounts());
        }
        [Ignore("Unit designed as untestable, refactoring requried to enable testing of this unit")]
        [Test]
        public async Task ItShouldNotThrowAndExceptionWhenCallingTheAccountsImageMethod()
        {
            Assert.DoesNotThrow(async () => ActionResultResponse = await Unit.AccountsImage());
        }
        [Ignore("Unit designed as untestable, refactoring requried to enable testing of this unit")]
        [Test]
        public async Task ItShouldNotThrowAndExceptionWhenCallingTheEmployerUserMethod()
        {
            Assert.DoesNotThrow(async () => ActionResultResponse = await Unit.EmployerUser());
        }
        [Ignore("Unit designed as untestable, refactoring requried to enable testing of this unit")]
        [Test]
        public async Task ItShouldNotThrowAndExceptionWhenCallingTheEmployerUserImageMethod()
        {
            Assert.DoesNotThrow(async () => ActionResultResponse = await Unit.EmployerUserImage());
        }
        [Ignore("Unit designed as untestable, refactoring requried to enable testing of this unit")]
        [Test]
        public async Task ItShouldNotThrowAndExceptionWhenCallingTheHealthImageMethod()
        {
            Assert.DoesNotThrow(async () => ActionResultResponse = await Unit.HealthImage());
        }
    }
}