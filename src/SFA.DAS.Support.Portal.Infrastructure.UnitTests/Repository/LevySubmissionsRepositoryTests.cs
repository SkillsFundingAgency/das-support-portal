using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using HMRC.ESFA.Levy.Api.Client;
using HMRC.ESFA.Levy.Api.Types;
using HMRC.ESFA.Levy.Api.Types.Exceptions;
using Moq;
using NUnit.Framework;
using SFA.DAS.NLog.Logger;
using SFA.DAS.Support.Portal.Core.Domain.Exceptions;

namespace SFA.DAS.Support.Portal.Infrastructure.UnitTests.Repository
{
    [TestFixture]
    public class LevySubmissionsRepositoryTests
    {
        [SetUp]
        public void Init()
        {
            _mockLogger = new Mock<ILog>();
            _mockApprenticeshipLevyApliClient = new Mock<IApprenticeshipLevyApiClient>();

            _sut = new LevySubmissionsRepository(
                _mockLogger.Object,
                _mockApprenticeshipLevyApliClient.Object);

            _levyDeclarations = new LevyDeclarations
            {
                EmpRef = "12345",
                Declarations = new List<Declaration>
                {
                    new Declaration
                    {
                        SubmissionTime = DateTime.Now.AddDays(2),
                        Id = "5"
                    },
                    new Declaration
                    {
                        SubmissionTime = DateTime.Now.AddDays(2),
                        Id = "6"
                    },
                    new Declaration
                    {
                        SubmissionTime = DateTime.Now,
                        Id = "4"
                    },
                    new Declaration
                    {
                        SubmissionTime = new DateTime(2017, 04, 1),
                        Id = "3"
                    },
                    new Declaration
                    {
                        SubmissionTime = new DateTime(2017, 03, 31, 23, 59, 59),
                        Id = "2"
                    },
                    new Declaration
                    {
                        SubmissionTime = new DateTime(2017, 03, 1),
                        Id = "1"
                    }
                }
            };
        }

        private Mock<ILog> _mockLogger;
        private LevySubmissionsRepository _sut;

        private Mock<IApprenticeshipLevyApiClient> _mockApprenticeshipLevyApliClient;
        private LevyDeclarations _levyDeclarations;

        [Test]
        public async Task ItShouldThrowAnApiHttpExceptionWhenApiClientThrowsApiHttpExceptionWhenNotNotFoundStatus()
        {
            _mockApprenticeshipLevyApliClient
                .Setup(x => x.GetEmployerLevyDeclarations(It.IsAny<string>(), null, null))
                .ThrowsAsync(new ApiHttpException((int) HttpStatusCode.BadRequest, "", "/api/whatever", "body", null));

            Assert.ThrowsAsync<ApiHttpException>(() => _sut.Get("123/4567"));
        }

        [Test]
        public async Task ItShouldThrowAnEntityNotFoundExceptionWhenApiClientThrowsApiHttpExceptionWithNotFoundStatus()
        {
            _mockApprenticeshipLevyApliClient
                .Setup(x => x.GetEmployerLevyDeclarations(It.IsAny<string>(), null, null))
                .ThrowsAsync(new ApiHttpException((int) HttpStatusCode.NotFound, "", "/api/whatever", "body", null));

            Assert.ThrowsAsync<EntityNotFoundException>(() => _sut.Get("123/4567"));
        }

        [Test]
        public async Task ShouldReturnFilteredLevyDeclarations()
        {
            _mockApprenticeshipLevyApliClient
                .Setup(x => x.GetEmployerLevyDeclarations(It.IsAny<string>(), null, null))
                .Returns(Task.FromResult(_levyDeclarations));

            var result = await _sut.Get("123/4567");

            result.Should().NotBeNull();
            result.Declarations.Count.Should().Be(4);
            result.EmpRef.Should().Be("12345");
            result.Declarations.First().Id.Should().Be("6");
        }
    }
}