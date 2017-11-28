using System.Collections.Generic;

namespace SFA.DAS.Portal.Infrastructure.DependencyResolution
{
    public interface ILoggingPropertyFactory
    {
        IDictionary<string, object> GetProperties();
    }
}