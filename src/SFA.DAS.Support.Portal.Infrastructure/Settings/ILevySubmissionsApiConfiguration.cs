using SFA.DAS.TokenService.Api.Client;

namespace SFA.DAS.Support.Portal.Infrastructure.Settings
{
    public interface ILevySubmissionsApiConfiguration : ITokenServiceApiClientConfiguration
    {
        string LevyTokenCertificatethumprint { get; set; }
    }
}