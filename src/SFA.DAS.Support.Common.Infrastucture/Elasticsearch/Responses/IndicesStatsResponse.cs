using System.Collections.Generic;
using Nest;

namespace SFA.DAS.Support.Common.Infrastucture.Elasticsearch
{
    public class IndicesStatsResponse
    {
        public IReadOnlyDictionary<string, IndicesStats> Indices { get; set; }
    }
}