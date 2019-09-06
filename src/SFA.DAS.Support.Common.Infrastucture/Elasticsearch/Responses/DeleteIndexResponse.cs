using System;

namespace SFA.DAS.Support.Common.Infrastucture.Elasticsearch
{
    public class DeleteIndexResponse
    {
        public bool Acknowledged { get; set; }
        public Exception OriginalException { get; internal set; }
    }
}