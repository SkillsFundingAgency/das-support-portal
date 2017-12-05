using System.Collections.Generic;

namespace SFA.DAS.Support.Indexer.ApplicationServices.Settings
{
    public interface ISiteSettings
    {
        IEnumerable<string> Sites { get; }
       string EnvironmentName { get; }
    }
}