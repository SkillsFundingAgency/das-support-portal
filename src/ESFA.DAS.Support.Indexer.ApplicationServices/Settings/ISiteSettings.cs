using System.Collections.Generic;

namespace ESFA.DAS.Support.Indexer.ApplicationServices.Settings
{
    public interface ISiteSettings
    {
        IEnumerable<string> Sites { get; }
        string IndexName { get; }
    }
}