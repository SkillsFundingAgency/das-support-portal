using System.Diagnostics.CodeAnalysis;
using SFA.DAS.EmployerUsers.Api.Client;
using SFA.DAS.Support.Portal.Core.Services;

namespace SFA.DAS.Support.Portal.Infrastructure.Settings
{
    [ExcludeFromCodeCoverage]
    public class EmployerUsersApiConfiguration : IEmployerUsersApiConfiguration
    {
        private readonly IProvideSettings _settings;

        public EmployerUsersApiConfiguration(IProvideSettings settings)
        {
            _settings = settings;
        }

        public string ApiBaseUrl => _settings.GetSetting("EmpUserApiBaseUrl");
        public string ClientId => _settings.GetSetting("EmpUserApiClientId");
        public string ClientSecret => _settings.GetSetting("EmpUserApiClientSecret");
        public string IdentifierUri => _settings.GetSetting("EmpUserApiIdentifierUri");
        public string Tenant => _settings.GetSetting("EmpUserApiTenant");
        public string ClientCertificateThumbprint => _settings.GetNullableSetting("EmpUserApiCertificateThumbprint");
    }
}
