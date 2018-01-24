using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace SFA.DAS.Support.Common.Infrastucture.Settings
{
    public class ElasticSearchSettings : ISearchSettings
    {
        [JsonRequired] public string IndexName { get; set; }

        [JsonIgnore]
        public IEnumerable<Uri> ElasticServerUrls
        {
            get
            {
                return ServerUrls.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries)
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .Select(x => new Uri(x));
            }
        }

        [JsonRequired] public string ServerUrls { get; set; }

        [JsonRequired] public string ElasticUsername { get; set; }

        [JsonRequired] public string ElasticPassword { get; set; }

        [JsonRequired] public bool Elk5Enabled { get; set; }

        [JsonRequired] public bool IgnoreSslCertificateEnabled { get; set; }

        [JsonRequired] public int IndexShards { get; set; }

        [JsonRequired] public int IndexReplicas { get; set; }

        [JsonRequired] public int IndexCopyCount { get; set; }
    }
}