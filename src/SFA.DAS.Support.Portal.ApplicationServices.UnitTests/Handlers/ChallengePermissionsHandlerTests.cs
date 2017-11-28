using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Support.Portal.ApplicationServices.Handlers;
using SFA.DAS.Support.Portal.ApplicationServices.Queries;
using SFA.DAS.Support.Portal.Core.Domain.Model;

namespace SFA.DAS.Support.Portal.ApplicationServices.UnitTests.Handlers
{
    [TestFixture]
    public class ChallengePermissionsHandlerTests
    {
        [SetUp]
        public void SetUp()
        {
            _mockAccountRepository = new Mock<IAccountRepository>();
            _mockChallengeRepository = new Mock<IChallengeRepository>();
            _sut = new ChallengePermissionHandler(_mockAccountRepository.Object, _mockChallengeRepository.Object);
        }

        private Mock<IAccountRepository> _mockAccountRepository;
        private Mock<IChallengeRepository> _mockChallengeRepository;
        private ChallengePermissionHandler _sut;

        [TestCase("a", "b", "0")]
        [TestCase("a", "b", "0.0")]
        [TestCase("a", "b", "0£")]
        [TestCase("a", "b", "£0")]
        [TestCase("a", "b", "£0.0")]
        [TestCase("a", "b", "0.0£")]
        [TestCase("a", "b", "5")]
        [TestCase("a", "b", "5.64")]
        public async Task ShouldReturnTrueIfEntryIsValid(string challengeElement1, string challengeElement2, string balance)
        {
            _mockAccountRepository.Setup(x => x.Get(It.IsAny<string>(), AccountFieldsSelection.ChallengePayeSchemes)).Returns(Task.FromResult(new Account()));

            _mockChallengeRepository.Setup(x => x.CheckData(It.IsAny<Account>(), It.IsAny<ChallengePermissionQuery>())).Returns(Task.FromResult(true));

            //Arrange
            var challengeEntry = new ChallengeEntry
            {
                Id = "qwerty",
                Balance = balance,
                Url = string.Empty,
                FirstCharacterPosition = "1",
                SecondCharacterPosition = "2",
                Challenge1 = challengeElement1,
                Challenge2 = challengeElement2
            };

            var message = new ChallengePermissionQuery(challengeEntry);

            //Act
            var result = await _sut.Handle(message);

            //Assert
            result.IsValid.Should().Be(true);
        }

        [TestCase("a", "b", "c")]
        [TestCase("a", "b", "0,0")]
        [TestCase("", "b", "0")]
        [TestCase("a", "", "0")]
        [TestCase("ab", "b", "1")]
        [TestCase("a", "ba", "1")]
        public async Task ShouldReturnFalseIfEntryIsInvalid(string challengeElement1, string challengeElement2, string balance)
        {
            _mockAccountRepository.Setup(x => x.Get(It.IsAny<string>(), AccountFieldsSelection.ChallengePayeSchemes)).Returns(Task.FromResult(new Account()));

            _mockChallengeRepository.Setup(x => x.CheckData(It.IsAny<Account>(), It.IsAny<ChallengePermissionQuery>())).Returns(Task.FromResult(true));

            //Arrange
            var challengeEntry = new ChallengeEntry
            {
                Id = "qwerty",
                Balance = balance,
                Url = string.Empty,
                FirstCharacterPosition = "1",
                SecondCharacterPosition = "2",
                Challenge1 = challengeElement1,
                Challenge2 = challengeElement2
            };

            var message = new ChallengePermissionQuery(challengeEntry);

            //Act
            var result = await _sut.Handle(message);

            //Assert
            result.IsValid.Should().Be(false);
        }
    }
}