using System.Collections.Generic;

namespace SFA.DAS.Support.Portal.Infrastructure.DependencyResolution
{
    public interface ILoggingPropertyFactory
    {
        IDictionary<string, object> GetProperties();
    }
}