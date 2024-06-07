using System.Threading.Tasks;
using System.Web.Mvc;
using FluentAssertions.Execution;
using FluentAssertions.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.NLog.Logger;
using SFA.DAS.Support.Portal.ApplicationServices.Models;
using SFA.DAS.Support.Portal.ApplicationServices.Services;
using SFA.DAS.Support.Portal.Web.Api.Models;
using SFA.DAS.Support.Portal.Web.Services;
using SFA.DAS.Support.Shared.Discovery;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Support.Portal.UnitTests.Web.Controllers.ResourceController
{
    public class WhenCallingTheResourceControllerChangeRoleGet
    {
        [Test, MoqAutoData]
        public async Task ReturnsCorrectViewAndModel(
            Mock<IManifestRepository> repository,
            Mock<ICheckPermissions> permissionsChecker,
            Mock<IGrantPermissions> permissionsGranter,
            Mock<IServiceConfiguration> config,
            Mock<ILog> logger,
            string hashedAccountId,
            string userRef,
            Role role,
            NavViewModel navViewModel,
            ResourceResultModel headerModel,
            ResourceResultModel resourceModel)
        {
            repository.Setup(x => x.GetNav(SupportServiceResourceKey.EmployerAccountChangeRole, hashedAccountId)).ReturnsAsync(navViewModel);
            repository.Setup(x => x.GenerateHeader(SupportServiceResourceKey.EmployerAccountChangeRole, hashedAccountId)).ReturnsAsync(headerModel);
            
            var sut = new Portal.Web.Controllers.ResourceController(repository.Object, permissionsChecker.Object, permissionsGranter.Object, config.Object);

            var actual = await sut.ChangeRole(hashedAccountId, userRef, role.ToString());

            using (new AssertionScope())
            {
                actual.Should().NotBeNull();

                actual.Should().BeOfType<RedirectToRouteResult>();

                repository.Verify(x => x.GetNav(SupportServiceResourceKey.EmployerAccountChangeRole, hashedAccountId), Times.Once);
                repository.Verify(x => x.GenerateHeader(SupportServiceResourceKey.EmployerAccountChangeRole, hashedAccountId), Times.Once);
                repository.Verify(x => x.SubmitChangeRoleRequest(hashedAccountId, userRef, role.ToString()), Times.Once);
            }
        }
    }
}