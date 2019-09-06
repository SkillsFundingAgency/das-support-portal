using System;

namespace SFA.DAS.Support.Common.Infrastucture.Elasticsearch
{
    public class CreateIndexResponse
    {
        public int? HttpStatusCode { get; set; }
        public Exception OriginalException { get; internal set; }
        public string DebugInformation { get; internal set; }
    }
}