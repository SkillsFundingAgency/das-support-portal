using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EAS.Account.Api.Types;
using SFA.DAS.Support.Portal.ApplicationServices;
using SFA.DAS.Support.Portal.ApplicationServices.Queries;
using SFA.DAS.Support.Portal.Core.Domain.Model;

namespace SFA.DAS.Support.Portal.Infrastructure.UnitTests.Repository
{
    [TestFixture]
    public class ChallengeRepositoryTests
    {
        [SetUp]
        public void Init()
        {
            _mockAccountRepository = new Mock<IAccountRepository>();

            _sut = new ChallengeRepository(_mockAccountRepository.Object);
        }

        private Mock<IAccountRepository> _mockAccountRepository;
        private ChallengeRepository _sut;

        [TestCase("1", "3", "2", "4", "£0")]
        [TestCase("1", "3", "6", "4", "£0")]
        [TestCase("1", "3", "2", "B", "£0")]
        [TestCase("2", "4", "5", "3", "0")]
        [TestCase("2", "4", "A", "5", "0£")]
        [TestCase("2", "4", "a", "5", "£0")]
        [TestCase("1", "5", "2", "6", "0")]
        [TestCase("1", "5", "2", "6", "0.0")]
        public async Task ShouldReturnTrueIfDataIsCorrect(string firstPosition, string secondPosition,
            string challengeElement1, string challengeElement2, string balance)
        {
            _mockAccountRepository.Setup(x => x.GetAccountBalance(It.IsAny<string>()))
                .Returns(Task.FromResult(0m));

            var account = new Account
            {
                PayeSchemes = new List<PayeSchemeViewModel>
                {
                    new PayeSchemeViewModel
                    {
                        Ref = "123/4567"
                    },
                    new PayeSchemeViewModel
                    {
                        Ref = "765/4321"
                    },
                    new PayeSchemeViewModel
                    {
                        Ref = "12A/B567"
                    }
                }
            };

            var challengeEntry = new ChallengeEntry
            {
                Balance = balance,
                Challenge1 = challengeElement1,
                Challenge2 = challengeElement2,
                Id = "AccountId",
                Url = "Url",
                FirstCharacterPosition = firstPosition,
                SecondCharacterPosition = secondPosition
            };

            var challengePermissionQuery = new ChallengePermissionQuery(challengeEntry);

            var result = await _sut.CheckData(account, challengePermissionQuery);

            result.Should().BeTrue();
        }

        [TestCase("1", "3", "2", "4", "1")]
        [TestCase("1", "3", "2", "4", "0€")]
        [TestCase("1", "3", "2", "4", "€0")]
        [TestCase("1", "3", "6", "B", "0")]
        [TestCase("2", "4", "5", "4", "0")]
        [TestCase("2", "4", "5", "4", "0,0")]
        [TestCase("2", "4", "5", "4", "papaya")]
        [TestCase("2", "4", "5", "4", "")]
        public async Task ShouldReturnFalseIfDataIsIncorrect(string firstPosition, string secondPosition,
            string challengeElement1, string challengeElement2, string balance)
        {
            _mockAccountRepository.Setup(x => x.GetAccountBalance(It.IsAny<string>()))
                .Returns(Task.FromResult(0m));

            var account = new Account
            {
                PayeSchemes = new List<PayeSchemeViewModel>
                {
                    new PayeSchemeViewModel
                    {
                        Ref = "123/4567"
                    },
                    new PayeSchemeViewModel
                    {
                        Ref = "765/4321"
                    },
                    new PayeSchemeViewModel
                    {
                        Ref = "12A/B567"
                    }
                }
            };

            var challengeEntry = new ChallengeEntry
            {
                Balance = balance,
                Challenge1 = challengeElement1,
                Challenge2 = challengeElement2,
                Id = "AccountId",
                Url = "Url",
                FirstCharacterPosition = firstPosition,
                SecondCharacterPosition = secondPosition
            };

            var challengePermissionQuery = new ChallengePermissionQuery(challengeEntry);

            var result = await _sut.CheckData(account, challengePermissionQuery);

            result.Should().BeFalse();
        }

        [TestCase("1", "5", "2", "7", "10.2575")]
        public async Task ShouldTruncateBalance(string firstPosition, string secondPosition, string challengeElement1,
            string challengeElement2, string balance)
        {
            _mockAccountRepository.Setup(x => x.GetAccountBalance(It.IsAny<string>()))
                .Returns(Task.FromResult(10m));

            var account = new Account
            {
                PayeSchemes = new List<PayeSchemeViewModel>
                {
                    new PayeSchemeViewModel
                    {
                        Ref = "421/9874"
                    }
                }
            };

            var challengeEntry = new ChallengeEntry
            {
                Balance = balance,
                Challenge1 = challengeElement1,
                Challenge2 = challengeElement2,
                Id = "AccountId",
                Url = "Url",
                FirstCharacterPosition = firstPosition,
                SecondCharacterPosition = secondPosition
            };

            var challengePermissionQuery = new ChallengePermissionQuery(challengeEntry);

            var result = await _sut.CheckData(account, challengePermissionQuery);

            result.Should().BeTrue();
        }

        [TestCase("1", "5", "2", "7", "-10.2575")]
        public async Task ShouldReturnTrueForNegativeBalance(string firstPosition, string secondPosition,
            string challengeElement1, string challengeElement2, string balance)
        {
            _mockAccountRepository.Setup(x => x.GetAccountBalance(It.IsAny<string>()))
                .Returns(Task.FromResult(-10m));

            var account = new Account
            {
                PayeSchemes = new List<PayeSchemeViewModel>
                {
                    new PayeSchemeViewModel
                    {
                        Ref = "421/9874"
                    }
                }
            };

            var challengeEntry = new ChallengeEntry
            {
                Balance = balance,
                Challenge1 = challengeElement1,
                Challenge2 = challengeElement2,
                Id = "AccountId",
                Url = "Url",
                FirstCharacterPosition = firstPosition,
                SecondCharacterPosition = secondPosition
            };

            var challengePermissionQuery = new ChallengePermissionQuery(challengeEntry);

            var result = await _sut.CheckData(account, challengePermissionQuery);

            result.Should().BeTrue();
        }

        [TestCase("1", "5", "2", "7", "10.2575")]
        public async Task ShouldReturnFalseIfBalanceDidNotMatch(string firstPosition, string secondPosition,
            string challengeElement1, string challengeElement2, string balance)
        {
            _mockAccountRepository.Setup(x => x.GetAccountBalance(It.IsAny<string>()))
                .Returns(Task.FromResult(-10.25m));

            var account = new Account
            {
                PayeSchemes = new List<PayeSchemeViewModel>
                {
                    new PayeSchemeViewModel
                    {
                        Ref = "421/9874"
                    }
                }
            };

            var challengeEntry = new ChallengeEntry
            {
                Balance = balance,
                Challenge1 = challengeElement1,
                Challenge2 = challengeElement2,
                Id = "AccountId",
                Url = "Url",
                FirstCharacterPosition = firstPosition,
                SecondCharacterPosition = secondPosition
            };

            var challengePermissionQuery = new ChallengePermissionQuery(challengeEntry);

            var result = await _sut.CheckData(account, challengePermissionQuery);

            result.Should().BeFalse();
        }
    }
}