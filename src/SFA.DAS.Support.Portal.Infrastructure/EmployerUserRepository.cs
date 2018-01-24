using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.EAS.Account.Api.Client;
using SFA.DAS.EAS.Account.Api.Types;
using SFA.DAS.EmployerUsers.Api.Client;
using SFA.DAS.EmployerUsers.Api.Types;
using SFA.DAS.NLog.Logger;
using SFA.DAS.Support.Portal.ApplicationServices;
using SFA.DAS.Support.Portal.ApplicationServices.Models;
using SFA.DAS.Support.Portal.Core.Domain.Model;

namespace SFA.DAS.Support.Portal.Infrastructure
{
    /// <summary>
    ///     TODO: Rationalise by way of;
    ///     Injecting a Mapper.
    ///     Convert logger strings to use nameof()
    ///     Configure/Inject the PageSize so as to open out for user ux based preferences
    /// </summary>
    public sealed class EmployerUserRepository : IEmployerUserRepository
    {
        private const int PageSize = 10;
        private readonly IAccountApiClient _accountApiClient;
        private readonly IEmployerUsersApiClient _client;
        private readonly ILog _logger;

        public EmployerUserRepository(ILog logger, IEmployerUsersApiClient client, IAccountApiClient accountApiClient)
        {
            _logger = logger;
            _client = client;
            _accountApiClient = accountApiClient;
        }

        public async Task<EmployerUserSearchResults> Search(string searchTerm, int page)
        {
            _logger.Debug(
                $"{nameof(IEmployerUsersApiClient)}.{nameof(IEmployerUsersApiClient.SearchEmployerUsers)}(\"{searchTerm}\",{page},{PageSize});");

            var response = await _client.SearchEmployerUsers(searchTerm, page, PageSize);

            var results = MapToEmployerUserSummary(response.Data).ToList();

            foreach (var employerUserSummary in results)
            {
                _logger.Debug(
                    $@"{nameof(IAccountApiClient)}.{nameof(IAccountApiClient.GetUserAccounts)}({
                            employerUserSummary.Id
                        });");

                var accounts = await _accountApiClient.GetUserAccounts(employerUserSummary.Id);

                employerUserSummary.Accounts = accounts.ToList();
            }

            return new EmployerUserSearchResults
            {
                Page = response.Page,
                LastPage = response.TotalPages,
                Results = results
            };
        }

        public async Task<EmployerUser> Get(string id)
        {
            try
            {
                _logger.Debug(
                    $"{nameof(IEmployerUsersApiClient)}.{nameof(IEmployerUsersApiClient.GetResource)}<{nameof(UserViewModel)}>(\"/api/users/{id}\");");
                var response = await _client.GetResource<UserViewModel>($"/api/users/{id}");

                return MapToEmployerUser(response);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failure connecting to DAS Employer Users API");
            }

            return null;
        }

        private IEnumerable<EmployerUserSummary> MapToEmployerUserSummary(List<UserSummaryViewModel> data)
        {
            foreach (var userSummaryViewModel in data)
                yield return new EmployerUserSummary
                {
                    Id = userSummaryViewModel.Id,
                    FirstName = userSummaryViewModel.FirstName,
                    LastName = userSummaryViewModel.LastName,
                    Email = userSummaryViewModel.Email,
                    IsLocked = userSummaryViewModel.IsLocked,
                    IsActive = userSummaryViewModel.IsActive,
                    Href = userSummaryViewModel.Href,
                    Accounts = new List<AccountDetailViewModel>()
                };
        }

        private EmployerUser MapToEmployerUser(UserViewModel data)
        {
            return new EmployerUser
            {
                Id = data.Id,
                FirstName = data.FirstName,
                LastName = data.LastName,
                Email = data.Email,
                IsActive = data.IsActive,
                FailedLoginAttempts = data.FailedLoginAttempts,
                IsLocked = data.IsLocked
            };
        }
    }
}