using System;
using System.Collections.Generic;

namespace SFA.DAS.Support.Deployment.Validator
{


    public partial class StatusClient
    {
        public class StatusData
        {
            public string ServiceName { get; set; }
            public string ServiceVersion { get; set; }
            public DateTimeOffset ServiceTime { get; set; }
            public string Request { get; set; }
            public Dictionary<string, SubServiceStatusData> SubSites { get; set; }
        }
    }
}