using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.EAS.Account.Api.Client;
using SFA.DAS.EAS.Account.Api.Types;
using SFA.DAS.EmployerUsers.Api.Client;
using SFA.DAS.EmployerUsers.Api.Types;
using SFA.DAS.NLog.Logger;
using SFA.DAS.Support.Portal.ApplicationServices;

namespace SFA.DAS.Support.Portal.Infrastructure.UnitTests.Repository
{
    [TestFixture]
    public class EmployerUserRepositoryTests
    {
        [SetUp]
        public void Setup()
        {
            _nLogLogger = new Mock<ILog>();
            _employerUsersApiClient = new Mock<IEmployerUsersApiClient>();
            _accountApiClient = new Mock<IAccountApiClient>();
            _unit = new EmployerUserRepository(
                _nLogLogger.Object,
                _employerUsersApiClient.Object,
                _accountApiClient.Object);
        }

        private IEmployerUserRepository _unit;
        private Mock<ILog> _nLogLogger;
        private Mock<IEmployerUsersApiClient> _employerUsersApiClient;
        private Mock<IAccountApiClient> _accountApiClient;

        [Test]
        public async Task ItShouldReturnResultsWhenThereAreSearchMatches()
        {
            var page = 1;
            var searchTerm = "something";

            var searchResponse = new EmployerUsers.Api.Types.PagedApiResponseViewModel<UserSummaryViewModel>
            {
                Data = new List<UserSummaryViewModel>
                {
                    new UserSummaryViewModel
                    {
                        Email = "1@2.com",
                        FirstName = "One",
                        LastName = "Two",
                        Href = "http://somelink/1",
                        Id = "1",
                        IsActive = true,
                        IsLocked = false
                    },
                    new UserSummaryViewModel
                    {
                        Email = "2@3.com",
                        FirstName = "Two",
                        LastName = "three",
                        Href = "http://somelink/2",
                        Id = "2",
                        IsActive = true,
                        IsLocked = false
                    }
                },
                Page = 1,
                TotalPages = 1
            };


            _employerUsersApiClient.Setup(e => e.SearchEmployerUsers(searchTerm, page, 10))
                .Returns(Task.FromResult(searchResponse));

            ICollection<AccountDetailViewModel> accountSearchResponse =
                new Collection<AccountDetailViewModel>
                {
                    new AccountDetailViewModel {AccountId = 1}
                };

            _accountApiClient.Setup(a => a.GetUserAccounts(It.IsAny<string>()))
                .Returns(Task.FromResult(accountSearchResponse));

            var response = await _unit.Search(searchTerm, page);
            
            _accountApiClient.Verify(a => a.GetUserAccounts(It.IsAny<string>()),
                Times.Exactly(searchResponse.Data.Count));


            _nLogLogger.Verify(l=>
                l.Debug(It.IsAny<string>()), Times.Exactly(searchResponse.Data.Count + 1));

            
            CollectionAssert.IsNotEmpty(response.Results);

        
        }

        [Test]
        public async Task ItShouldReturnNullAndLogExceptionIfExceptionIsThrown()
        {
            
            string id = "123123";

            _employerUsersApiClient.Setup(
                    e => e.GetResource<UserViewModel>(It.IsAny<string>()))
                .Throws(new Exception());

            var response = await _unit.Get(id);
            _nLogLogger.Verify(l => l.Debug(It.IsAny<string>()), Times.Once);

            _nLogLogger.Verify(l=>l.Error(It.IsAny<Exception>(), It.IsAny<string>()), Times.Once);

            Assert.IsNull(response);
        }

        [Test]
        public async Task ItShouldReturnSearchResultIfExceptionIsNotThrown()
        {

            string id = "123123";

            _employerUsersApiClient.Setup(
                    e => e.GetResource<UserViewModel>(It.IsAny<string>()))
                .Returns( Task.FromResult(new UserViewModel()
                {
                    
                }));

            var response = await _unit.Get(id);

            _nLogLogger.Verify(l => l.Debug(It.IsAny<string>()), Times.Once);

            _nLogLogger.Verify(l => l.Error(It.IsAny<Exception>(), It.IsAny<string>()), Times.Never);

            Assert.IsNotNull(response);



        }

    }
}