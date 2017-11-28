using System.Collections.Generic;

namespace SFA.DAS.Portal.ApplicationServices.Settings
{
    public interface ISiteSettings
    {
        IEnumerable<string> Sites { get; }
    }
}