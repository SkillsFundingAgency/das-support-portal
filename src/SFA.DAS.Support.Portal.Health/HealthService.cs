using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Threading.Tasks;
using SFA.DAS.EAS.Account.Api.Client;
using SFA.DAS.EmployerUsers.Api.Client;
using SFA.DAS.Support.Portal.Health.Model;

namespace SFA.DAS.Support.Portal.Health
{
    [ExcludeFromCodeCoverage]
    public class HealthService : IHealthService
    {
        private readonly IAccountApiClient _accountApiClient;
        private readonly IEmployerUsersApiClient _client;

        public HealthService(IEmployerUsersApiClient client, IAccountApiClient accountApiClient)
        {
            _client = client;
            _accountApiClient = accountApiClient;
        }

        public async Task<HealthModel> CreateHealthModel()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var version = assembly.GetName().Version.ToString();
            var fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);

            var response = new HealthModel
            {
                Version = fileVersionInfo.ProductVersion,
                AssemblyVersion = version
            };

            var employerUserModel = await CreateHealthEmployerUserModel();
            var accountModel = await CreateHealthAccountsModel();

            if (employerUserModel.ApiStatus == Status.Green &&
                accountModel.ApiStatus == Status.Green)
                response.ApiStatus = Status.Green;
            else
                response.ApiStatus = Status.Red;

            return response;
        }

        public async Task<HealthEmployerUserModel> CreateHealthEmployerUserModel()
        {
            return new HealthEmployerUserModel
            {
                ApiStatus = await GetEmployerUserApiStatus()
            };
        }

        public async Task<HealthAccountsModel> CreateHealthAccountsModel()
        {
            return new HealthAccountsModel
            {
                ApiStatus = await GetAccountsApiStatus()
            };
        }

        private async Task<Status> GetAccountsApiStatus()
        {
            try
            {
                var tsk = await _accountApiClient.GetPageOfAccounts();

                return Status.Green;
            }
            catch
            {
                return Status.Red;
            }
        }

        private async Task<Status> GetEmployerUserApiStatus()
        {
            try
            {
                var tsk = await _client.SearchEmployerUsers("a", 1, 10);

                return Status.Green;
            }
            catch
            {
                return Status.Red;
            }
        }
    }
}