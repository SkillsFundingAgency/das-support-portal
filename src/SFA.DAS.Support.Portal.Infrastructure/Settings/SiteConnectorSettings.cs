using System.Configuration;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.Support.Portal.Infrastructure.Settings
{
    [ExcludeFromCodeCoverage]
    public sealed class SiteConnectorSettings : ISiteConnectorSettings
    {
        public string ClientId => ConfigurationManager.AppSettings["ClientId"];
        public string AppKey => ConfigurationManager.AppSettings["AppKey"];
        public string ResourceId => ConfigurationManager.AppSettings["ResourceId"];
        public string Tenant => ConfigurationManager.AppSettings["Tenant"];

    }
}