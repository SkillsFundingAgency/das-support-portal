using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;
using SFA.DAS.EmployerUsers.Api.Client;

namespace SFA.DAS.Support.Portal.Infrastructure.Settings
{
    [ExcludeFromCodeCoverage]
    public class EmployerUsersApiConfiguration : IEmployerUsersApiConfiguration
    {
        [JsonRequired] public string ApiBaseUrl { get; set; }

        [JsonRequired] public string ClientId { get; set; }

        [JsonRequired] public string ClientSecret { get; set; }

        [JsonRequired] public string IdentifierUri { get; set; }

        [JsonRequired] public string Tenant { get; set; }

        [JsonRequired] public string ClientCertificateThumbprint { get; set; }
    }
}