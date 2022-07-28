using SFA.DAS.Support.Shared.Discovery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFA.DAS.Support.Shared.SiteConnection
{
    public interface ISubSiteConnectorSettings
    {
        List<SubSiteConnectorConfig> SubSiteConnectorSettings { get; }
    }

    public class SubSiteConnectorConfigs : ISubSiteConnectorSettings
    {
        public List<SubSiteConnectorConfig> SubSiteConnectorSettings { get; set; }
    }

    public class SubSiteConnectorConfig
    {
        public string Key { get; set; }
        public string BaseUrl { get; set; }
        public string IdentifierUri { get; set; }
    }
}