using System.Collections.Generic;

namespace SFA.DAS.Support.Portal.ApplicationServices.Settings
{
    public interface ISiteSettings
    {
        IEnumerable<string> Sites { get; }
    }
}