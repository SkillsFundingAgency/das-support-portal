using System.Collections.Generic;

namespace SFA.DAS.Support.Indexer.Worker.DependencyResolution
{
    public interface ILoggingPropertyFactory
    {
        IDictionary<string, object> GetProperties();
    }
}