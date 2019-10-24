using System.Collections.Generic;

namespace SFA.DAS.Support.Indexer.Jobs.DependencyResolution
{
    public interface ILoggingPropertyFactory
    {
        IDictionary<string, object> GetProperties();
    }
}