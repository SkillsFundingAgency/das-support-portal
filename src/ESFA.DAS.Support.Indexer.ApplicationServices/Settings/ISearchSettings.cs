using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESFA.DAS.Support.Indexer.ApplicationServices.Settings
{
    public interface ISearchSettings
    {
        string AdminApiKey { get; }
        string ServiceName { get; }
    }
}
