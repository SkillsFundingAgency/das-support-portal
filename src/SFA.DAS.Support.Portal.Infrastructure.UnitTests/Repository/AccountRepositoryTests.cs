using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EAS.Account.Api.Client;
using SFA.DAS.EAS.Account.Api.Types;
using SFA.DAS.NLog.Logger;
using SFA.DAS.Support.Common.Infrastucture.Indexer;
using SFA.DAS.Support.Portal.ApplicationServices.Services;
using SFA.DAS.Support.Portal.Core.Domain.Model;
using SFA.DAS.Support.Portal.Core.Helpers;
using SFA.DAS.Support.Portal.Core.Services;
using SFA.DAS.Support.Portal.Infrastructure.Services;
using SFA.DAS.Support.Shared;

namespace SFA.DAS.Support.Portal.Infrastructure.UnitTests.Repository
{

    [TestFixture]
    public class EntityRepositoryTests
    {
        private Mock<ISearchProvider> _mockSearchProvider;
        private IEntityRepository _unit;
        [SetUp]
        public void Setup()
        {
            _mockSearchProvider = new Mock<ISearchProvider>();
            _unit = new EntityRepository(_mockSearchProvider.Object);
        }

        [Test]
        public void ItShouldReturnTheSearchResultsAsAListOfTheSuppliedData()
        {
            var query = "somethig";
            var expected = new List<string>() { "<div>A hit 1</div>", "<div>A hit 2</div>", "<div>A hit 3</div>" };
            var searchResultItems = new List<SearchItem>()
            {
                new SearchItem(){ Html = "<div>A hit 1</div>"},
                new SearchItem(){ Html = "<div>A hit 2</div>"},
                new SearchItem(){ Html = "<div>A hit 3</div>"},
            };
            _mockSearchProvider
                .Setup(x => x.Search<SearchItem>(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(searchResultItems);

            var actual = _unit.Search(query).ToList();

            CollectionAssert.IsNotEmpty(actual);
            CollectionAssert.AreEqual(expected, actual);
        }

    }

    [TestFixture]
    public class AccountRepositoryTests
    {
        [SetUp]
        public void Init()
        {
            _mockLogger = new Mock<ILog>();
            _mockAccountApiClient = new Mock<IAccountApiClient>();
            _mockPayeObfuscator = new Mock<IPayeSchemeObfuscator>();
            _mockDatetimeService = new Mock<IDatetimeService>();

            _mockPayeObfuscator.Setup(x => x.ObscurePayeScheme(It.IsAny<string>())).Returns("12345");

            _sut = new AccountRepository(
                _mockLogger.Object,
                _mockAccountApiClient.Object,
                _mockPayeObfuscator.Object,
                _mockDatetimeService.Object
            );

            IEnumerable<ResourceViewModel> resourceViewModelList = new List<ResourceViewModel>
            {
                new ResourceViewModel
                {
                    Id = "1234",
                    Href = "href/1234"
                }
            };

            IEnumerable<ResourceViewModel> legalentityresourceViewModelList = new List<ResourceViewModel>
            {
                new ResourceViewModel
                {
                    Id = "1",
                    Href = "href/1"
                },
                new ResourceViewModel
                {
                    Id = "2",
                    Href = "href/2"
                },
                new ResourceViewModel
                {
                    Id = "3",
                    Href = "href/3"
                },
                new ResourceViewModel
                {
                    Id = "4",
                    Href = "href/4"
                },
                new ResourceViewModel
                {
                    Id = "5",
                    Href = "href/5"
                },
                new ResourceViewModel
                {
                    Id = "6",
                    Href = "href/NullOrEmpty"
                }
            };

            _mockedAccount = new AccountDetailViewModel
            {
                HashedAccountId = "HashedAccountId",
                DasAccountName = "DasAccountName",
                PayeSchemes = new ResourceList(resourceViewModelList),
                LegalEntities = new ResourceList(legalentityresourceViewModelList)
            };
        }

        private Mock<ILog> _mockLogger;
        private Mock<IAccountApiClient> _mockAccountApiClient;
        private AccountRepository _sut;

        private AccountDetailViewModel _mockedAccount;
        private Mock<IPayeSchemeObfuscator> _mockPayeObfuscator;
        private Mock<IDatetimeService> _mockDatetimeService;

        [Test]
        public async Task ShouldNotReturnLegalEntityIfExpiredOrRemoved()
        {
            var mockedlegalEntities = new List<LegalEntityViewModel>
            {
                new LegalEntityViewModel
                {
                    AgreementStatus = EmployerAgreementStatus.Signed
                },
                new LegalEntityViewModel
                {
                    AgreementStatus = EmployerAgreementStatus.Pending
                },
                new LegalEntityViewModel
                {
                    AgreementStatus = EmployerAgreementStatus.Superseded
                },
                new LegalEntityViewModel
                {
                    AgreementStatus = EmployerAgreementStatus.Removed
                },
                new LegalEntityViewModel
                {
                    AgreementStatus = EmployerAgreementStatus.Expired
                },
                new LegalEntityViewModel
                {
                    Name = "EmployerAgreementStatus is null or empty"
                }
            };

            _mockAccountApiClient.Setup(x =>
                    x.GetResource<AccountDetailViewModel>(It.Is<string>(y => y == "/api/accounts/112344")))
                .Returns(Task.FromResult(_mockedAccount));

            _mockAccountApiClient.Setup(x => x.GetResource<LegalEntityViewModel>(It.Is<string>(y => y == "href/1")))
                .Returns(Task.FromResult(mockedlegalEntities[0]));
            _mockAccountApiClient.Setup(x => x.GetResource<LegalEntityViewModel>(It.Is<string>(y => y == "href/2")))
                .Returns(Task.FromResult(mockedlegalEntities[1]));
            _mockAccountApiClient.Setup(x => x.GetResource<LegalEntityViewModel>(It.Is<string>(y => y == "href/3")))
                .Returns(Task.FromResult(mockedlegalEntities[2]));
            _mockAccountApiClient.Setup(x => x.GetResource<LegalEntityViewModel>(It.Is<string>(y => y == "href/4")))
                .Returns(Task.FromResult(mockedlegalEntities[3]));
            _mockAccountApiClient.Setup(x => x.GetResource<LegalEntityViewModel>(It.Is<string>(y => y == "href/5")))
                .Returns(Task.FromResult(mockedlegalEntities[4]));
            _mockAccountApiClient.Setup(x =>
                    x.GetResource<LegalEntityViewModel>(It.Is<string>(y => y == "href/NullOrEmpty")))
                .Returns(Task.FromResult(mockedlegalEntities[5]));

            var result = await _sut.Get("112344", AccountFieldsSelection.Organisations);

            result.Should().NotBeNull();
            result.LegalEntities.Count().Should().Equals(3);
            result.LegalEntities.Any(x => x.AgreementStatus == EmployerAgreementStatus.Removed ||
                                          x.AgreementStatus == EmployerAgreementStatus.Expired ||
                                          x.AgreementStatus == 0).Should().BeFalse();
        }

        [Test]
        public async Task ShouldNotReturnPayeSchemeIfAddedDateIsInTheFuture()
        {
            var mockedPayeScheme = new PayeSchemeViewModel
            {
                DasAccountId = "DasAccountId",
                Name = "Name",
                AddedDate = DateTime.UtcNow.AddHours(1),
                RemovedDate = null,
                Ref = "12345678"
            };

            _mockAccountApiClient.Setup(x => x.GetResource<AccountDetailViewModel>(It.IsAny<string>()))
                .Returns(Task.FromResult(_mockedAccount));

            _mockAccountApiClient.Setup(x => x.GetResource<PayeSchemeViewModel>(It.IsAny<string>()))
                .Returns(Task.FromResult(mockedPayeScheme));

            var result = await _sut.Get("112344", AccountFieldsSelection.PayeSchemes);

            result.Should().NotBeNull();
            result.PayeSchemes.Count().Should().Equals(0);
        }

        [Test]
        public async Task ShouldNotReturnPayeSchemeIfRemovedDateIsInThePast()
        {
            var mockedPayeScheme = new PayeSchemeViewModel
            {
                DasAccountId = "DasAccountId",
                Name = "Name",
                AddedDate = DateTime.UtcNow.AddHours(-2),
                RemovedDate = DateTime.UtcNow.AddHours(-1),
                Ref = "12345678"
            };

            _mockAccountApiClient.Setup(x => x.GetResource<AccountDetailViewModel>(It.IsAny<string>()))
                .Returns(Task.FromResult(_mockedAccount));

            _mockAccountApiClient.Setup(x => x.GetResource<PayeSchemeViewModel>(It.IsAny<string>()))
                .Returns(Task.FromResult(mockedPayeScheme));

            var result = await _sut.Get("112344", AccountFieldsSelection.PayeSchemes);

            result.Should().NotBeNull();
            result.PayeSchemes.Count().Should().Equals(0);
        }

        [Test]
        public async Task ShouldReturnObscuredPayeSchemeRef()
        {
            var mockedPayeScheme = new PayeSchemeViewModel
            {
                DasAccountId = "DasAccountId",
                Name = "Name",
                AddedDate = DateTime.UtcNow.AddHours(-2),
                RemovedDate = null,
                Ref = "12345678"
            };

            _mockAccountApiClient.Setup(x => x.GetResource<AccountDetailViewModel>(It.IsAny<string>()))
                .Returns(Task.FromResult(_mockedAccount));

            _mockAccountApiClient.Setup(x => x.GetResource<PayeSchemeViewModel>(It.IsAny<string>()))
                .Returns(Task.FromResult(mockedPayeScheme));

            _mockPayeObfuscator.Setup(x => x.ObscurePayeScheme("12345678")).Returns("1******8");

            var result = await _sut.Get("112344", AccountFieldsSelection.PayeSchemes);

            result.Should().NotBeNull();
            result.PayeSchemes.Count().Should().Equals(1);
            result.PayeSchemes.First().Ref.Should().Be("1******8");
        }

        [Test]
        public async Task ShouldReturnPayeSchemeIntoAccountIfAddedDateIsInThePastAndRemovedDateIsNull()
        {
            var mockedPayeScheme = new PayeSchemeViewModel
            {
                DasAccountId = "DasAccountId",
                Name = "Name",
                AddedDate = DateTime.UtcNow.AddHours(-1),
                RemovedDate = null,
                Ref = "12345678"
            };

            _mockAccountApiClient.Setup(x => x.GetResource<AccountDetailViewModel>(It.IsAny<string>()))
                .Returns(Task.FromResult(_mockedAccount));

            _mockAccountApiClient.Setup(x => x.GetResource<PayeSchemeViewModel>(It.IsAny<string>()))
                .Returns(Task.FromResult(mockedPayeScheme));

            var result = await _sut.Get("112344", AccountFieldsSelection.PayeSchemes);

            result.Should().NotBeNull();
            result.PayeSchemes.Count().Should().Equals(1);
        }

        [Test]
        public async Task ShouldReturnPayeSchemeIntoAccountIfDatesAreValid()
        {
            var mockedPayeScheme = new PayeSchemeViewModel
            {
                DasAccountId = "DasAccountId",
                Name = "Name",
                AddedDate = DateTime.UtcNow.AddHours(-1),
                RemovedDate = DateTime.UtcNow.AddHours(1),
                Ref = "12345678"
            };

            _mockAccountApiClient.Setup(x => x.GetResource<AccountDetailViewModel>(It.IsAny<string>()))
                .Returns(Task.FromResult(_mockedAccount));

            _mockAccountApiClient.Setup(x => x.GetResource<PayeSchemeViewModel>(It.IsAny<string>()))
                .Returns(Task.FromResult(mockedPayeScheme));

            var result = await _sut.Get("112344", AccountFieldsSelection.PayeSchemes);

            result.Should().NotBeNull();
            result.PayeSchemes.Count().Should().Equals(1);
        }

        [Test]
        public async Task ShouldReturnTeamMembers()
        {
            ICollection<TeamMemberViewModel> mockedPayeScheme = new List<TeamMemberViewModel>
            {
                new TeamMemberViewModel
                {
                    Email = "testEmail1@gmail.com",
                    Name = "Test1",
                    Role = "Owner",
                    UserRef = "href/1"
                },
                new TeamMemberViewModel
                {
                    Email = "testEmail2@gmail.com",
                    Name = "Test2",
                    Role = "Role2",
                    UserRef = "href/2"
                },
                new TeamMemberViewModel
                {
                    Email = "testEmail3@gmail.com",
                    Name = "Test3",
                    Role = "Viewer",
                    UserRef = "href/3"
                }
            };

            _mockAccountApiClient.Setup(x => x.GetResource<AccountDetailViewModel>(It.IsAny<string>()))
                .Returns(Task.FromResult(_mockedAccount));

            _mockAccountApiClient.Setup(x => x.GetAccountUsers(It.IsAny<string>()))
                .Returns(Task.FromResult(mockedPayeScheme));

            var result = await _sut.Get("112344", AccountFieldsSelection.TeamMembers);

            result.Should().NotBeNull();
            result.TeamMembers.Count.Should().Equals(3);
            result.TeamMembers.First().Name.Equals("Test1");
        }
    }
}