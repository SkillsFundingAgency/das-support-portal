using System;
using System.Collections.Generic;

namespace SFA.DAS.Support.Portal.Core.Configuration
{
    public interface IConfigurationSettings
    {
        IEnumerable<Uri> ElasticServerUrls { get; }

        string EnvironmentName { get; }

        string ApplicationName { get; }
    }
}
