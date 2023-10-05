using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.EAS.Account.Api.Types;
using SFA.DAS.Support.Portal.ApplicationServices.Handlers;
using SFA.DAS.Support.Portal.ApplicationServices.Queries;
using SFA.DAS.Support.Portal.ApplicationServices.Responses;
using SFA.DAS.Support.Portal.ApplicationServices.Services;
using SFA.DAS.Support.Portal.Core.Domain.Model;

namespace SFA.DAS.Support.Portal.ApplicationServices.UnitTests.Handlers
{
    [TestFixture]
    public class ChallengeHandlerTests
    {
        [SetUp]
        public void Setup()
        {
            _mockAccountRepository = new Mock<IAccountRepository>();
            _mockChallengeService = new Mock<IChallengeService>();
            _unit = new ChallengeHandler(_mockAccountRepository.Object, _mockChallengeService.Object);
        }

        private ChallengeHandler _unit;
        private Mock<IAccountRepository> _mockAccountRepository;
        private Mock<IChallengeService> _mockChallengeService;


        [Test]
        public async Task ItShouldEstablishTheChallengeCharacterIndexesWhenTheAccountIsFound()
        {
            var id = "123123";
            var challengeQuery = new ChallengeQuery(id);

            var account = new Account
            {
                PayeSchemes = new List<PayeSchemeViewModel>
                {
                    new PayeSchemeViewModel {Name = "123/4567789"}
                }
            };

            _mockAccountRepository.Setup(r => r.Get(id, AccountFieldsSelection.PayeSchemes))
                .Returns(Task.FromResult(account));
            var characterIndexes = new List<int>();
            _mockChallengeService.Setup(c => c.GetPayeSchemesCharacters(account.PayeSchemes))
                .Returns(characterIndexes);

            var response = await _unit.Handle(challengeQuery, CancellationToken.None);

            CollectionAssert.AreEqual(characterIndexes, response.Characters);
        }

        [Test]
        public async Task ItShouldReturnNotFoundCodeCodeWhenAccountIsNotFound()
        {
            var id = "123123";
            var challengeQuery = new ChallengeQuery(id);


            _mockAccountRepository.Setup(r => r.Get(id, AccountFieldsSelection.PayeSchemes))
                .Returns(Task.FromResult(null as Account));

            var response = await _unit.Handle(challengeQuery, CancellationToken.None);

            Assert.AreEqual(SearchResponseCodes.NoSearchResultsFound, response.StatusCode);
        }

        [Test]
        public async Task ItShouldReturnSuccessCodeWhenAccountIsFound()
        {
            var id = "123123";
            var challengeQuery = new ChallengeQuery(id);

            var account = new Account();

            _mockAccountRepository.Setup(r => r.Get(id, AccountFieldsSelection.PayeSchemes))
                .Returns(Task.FromResult(account));

            var response = await _unit.Handle(challengeQuery, CancellationToken.None);

            Assert.AreEqual(SearchResponseCodes.Success, response.StatusCode);
        }
    }
}