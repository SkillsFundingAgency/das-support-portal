using System.Linq;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.Portal.ApplicationServices.Handlers;
using SFA.DAS.Portal.ApplicationServices.Models;
using SFA.DAS.Portal.ApplicationServices.Queries;
using SFA.DAS.Portal.Core.Domain.Model;

namespace SFA.DAS.Portal.ApplicationServices.UnitTests.Handlers
{
    [TestFixture]
    public class EmployerUserSearchHandlerTests
    {
        [SetUp]
        public void SetUp()
        {
            _mockSearchRepository = new Mock<IEmployerUserRepository>();
            _sut = new EmployerUserSearchHandler(_mockSearchRepository.Object);
        }

        private Mock<IEmployerUserRepository> _mockSearchRepository;
        private EmployerUserSearchHandler _sut;

        private readonly EmployerUserSearchQuery _arg = new EmployerUserSearchQuery
        {
            Page = 1,
            SearchTerm = "Luke"
        };

        [Test]
        public async Task CheckThatALockedUserHasALockedStatus()
        {
            //Arrange

            _mockSearchRepository.Setup(x => x.Search(_arg.SearchTerm, _arg.Page)).Returns(Task.FromResult(new EmployerUserSearchResults
            {
                SearchTerm = "Luke",
                Page = 1,
                LastPage = 1,
                Results = new[]
                {
                    new EmployerUserSummary
                    {
                        Id = "2",
                        Href = "/user/2",
                        Email = "test@blog.co.uk",
                        FirstName = "Luke",
                        LastName = "Rayner",
                        IsActive = true,
                        IsLocked = true
                    }
                }
            }));

            //Act
            var result = await _sut.Handle(_arg);

            //Assert
            Assert.AreEqual(result.Results.First().Status, UserStatus.Locked);
        }

        [Test]
        public async Task CheckThatAnActiveUserHasAnActiveStatus()
        {
            //Arrange

            _mockSearchRepository.Setup(x => x.Search(_arg.SearchTerm, _arg.Page)).Returns(Task.FromResult(new EmployerUserSearchResults
            {
                SearchTerm = "Luke",
                Page = 1,
                LastPage = 1,
                Results = new[]
                {
                    new EmployerUserSummary
                    {
                        Id = "2",
                        Href = "/user/2",
                        Email = "test@blog.co.uk",
                        FirstName = "Luke",
                        LastName = "Rayner",
                        IsActive = true,
                        IsLocked = false
                    }
                }
            }));

            //Act
            var result = await _sut.Handle(_arg);

            //Assert
            Assert.AreEqual(result.Results.First().Status, UserStatus.Active);
        }

        [Test]
        public async Task CheckThatAnInActiveUserHasAnUnverifiedStatus()
        {
            //Arrange

            _mockSearchRepository.Setup(x => x.Search(_arg.SearchTerm, _arg.Page)).Returns(Task.FromResult(new EmployerUserSearchResults
            {
                SearchTerm = "Luke",
                Page = 1,
                LastPage = 1,
                Results = new[]
                {
                    new EmployerUserSummary
                    {
                        Id = "2",
                        Href = "/user/2",
                        Email = "test@blog.co.uk",
                        FirstName = "Luke",
                        LastName = "Rayner",
                        IsActive = false,
                        IsLocked = false
                    }
                }
            }));

            //Act
            var result = await _sut.Handle(_arg);

            //Assert
            Assert.AreEqual(result.Results.First().Status, UserStatus.Unverified);
        }


        [Test]
        [TestCaseSource(typeof(UserStatusTestDataClass), "UserStatusTestCases")]
        public async Task<UserStatus> UserStatusTest(bool active, bool locked)
        {
            _mockSearchRepository.Setup(x => x.Search(_arg.SearchTerm, _arg.Page)).Returns(Task.FromResult(new EmployerUserSearchResults
            {
                SearchTerm = "Luke",
                Page = 1,
                LastPage = 1,
                Results = new[]
                {
                    new EmployerUserSummary
                    {
                        Id = "2",
                        Href = "/user/2",
                        Email = "test@blog.co.uk",
                        FirstName = "Luke",
                        LastName = "Rayner",
                        IsActive = active,
                        IsLocked = locked
                    }
                }
            }));

            //Act
            var result = await _sut.Handle(_arg);

            return result.Results.First().Status;
        }
    }
}