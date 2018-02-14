using System.Collections.Generic;
using SFA.DAS.Support.Shared.Discovery;

namespace SFA.DAS.Support.Indexer.ApplicationServices.Settings
{
    public interface ISiteSettings
    {
        string BaseUrls { get; }
        string DelayTimeInSeconds { get; set; }
    }
}