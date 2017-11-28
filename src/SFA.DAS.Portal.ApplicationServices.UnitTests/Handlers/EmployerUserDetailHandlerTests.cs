using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Portal.ApplicationServices.Handlers;
using SFA.DAS.Portal.ApplicationServices.Queries;
using SFA.DAS.Portal.ApplicationServices.Responses;
using SFA.DAS.Portal.Core.Domain.Model;

namespace SFA.DAS.Portal.ApplicationServices.UnitTests.Handlers
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public sealed class EmployerUserDetailHandlerTests
    {
        [SetUp]
        public void Init()
        {
            _mockService = new Mock<IEmployerUserRepository>();
            _sut = new EmployerUserDetailHandler(_mockService.Object);
        }

        private Mock<IEmployerUserRepository> _mockService;
        private EmployerUserDetailHandler _sut;

        [Test]
        public async Task StatusCodeShouldBeNotFoundIfTheMessageIdIsNull()
        {
            _mockService.Setup(svc => svc.Get(It.Is<string>(x => string.IsNullOrEmpty(x))))
                .Returns(Task.FromResult((EmployerUser) null));

            var response = await _sut.Handle(new EmployerUserQuery(null));

            response.StatusCode.Should().Be(SearchResponseCodes.NoSearchResultsFound);
            response.User.Should().BeNull();
        }

        [Test]
        public async Task StatusCodeShouldBeNotFoundIfTheResponceIsNull()
        {
            _mockService.Setup(svc => svc.Get(It.IsAny<string>()))
                .Returns(Task.FromResult((EmployerUser) null));

            var response = await _sut.Handle(new EmployerUserQuery("2f42658b-aa1c-401e-8fa3-0cf41516f125"));

            response.StatusCode.Should().Be(SearchResponseCodes.NoSearchResultsFound);
            response.User.Should().BeNull();
        }

        [Test]
        public async Task UserStatusShouldBeActiveIfActiveAndNotLocked()
        {
            _mockService.Setup(svc => svc.Get(It.IsAny<string>()))
                .Returns(Task.FromResult(new EmployerUser
                {
                    FirstName = "Joe",
                    LastName = "Bloggs",
                    Email = "joe@bloggs.com",
                    IsActive = true,
                    IsLocked = false
                }));

            var response = await _sut.Handle(new EmployerUserQuery("1234"));

            response.User.Status.Should().Be(UserStatus.Active);
        }

        [Test]
        public async Task UserStatusShouldBeLockedIfActiveAndLocked()
        {
            _mockService.Setup(svc => svc.Get(It.IsAny<string>()))
                .Returns(Task.FromResult(new EmployerUser
                {
                    FirstName = "Joe",
                    LastName = "Bloggs",
                    Email = "joe@bloggs.com",
                    IsActive = true,
                    IsLocked = true
                }));

            var response = await _sut.Handle(new EmployerUserQuery("1234"));

            response.User.Status.Should().Be(UserStatus.Locked);
        }

        [Test]
        public async Task UserStatusShouldBeUnverifiedIfNotActiveAndNotLocked()
        {
            _mockService.Setup(svc => svc.Get(It.IsAny<string>()))
                .Returns(Task.FromResult(new EmployerUser
                {
                    FirstName = "Joe",
                    LastName = "Bloggs",
                    Email = "joe@bloggs.com",
                    IsActive = false,
                    IsLocked = false
                }));

            var response = await _sut.Handle(new EmployerUserQuery("1234"));

            response.User.Status.Should().Be(UserStatus.Unverified);
        }
    }
}