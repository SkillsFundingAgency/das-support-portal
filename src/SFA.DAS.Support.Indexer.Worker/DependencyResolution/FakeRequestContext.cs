﻿using SFA.DAS.NLog.Logger;

namespace SFA.DAS.Support.Indexer.Worker.DependencyResolution
{
    public class FakeRequestContext : IRequestContext
    {
        public string Url => string.Empty;

        public string IpAddress => string.Empty;
    }
}