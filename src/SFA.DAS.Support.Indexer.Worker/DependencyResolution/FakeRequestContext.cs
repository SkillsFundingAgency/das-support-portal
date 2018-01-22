using SFA.DAS.NLog.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFA.DAS.Support.Indexer.Worker.DependencyResolution
{
    public class FakeRequestContext : IRequestContext
    {
        public string Url => string.Empty;

        public string IpAddress => string.Empty;
    }
}
