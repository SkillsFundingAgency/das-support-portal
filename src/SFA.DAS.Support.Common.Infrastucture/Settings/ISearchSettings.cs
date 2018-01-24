using System;
using System.Collections.Generic;

namespace SFA.DAS.Support.Common.Infrastucture.Settings
{
    public interface ISearchSettings
    {
        string IndexName { get; set; }

        IEnumerable<Uri> ElasticServerUrls { get; }

        /// <summary>
        ///     A comma separated list of server Url's
        ///     e.g. 'https://localhost:43311,https://localhost:43312'
        /// </summary>
        string ServerUrls { get; set; }

        string ElasticUsername { get; set; }

        string ElasticPassword { get; set; }

        bool Elk5Enabled { get; set; }

        bool IgnoreSslCertificateEnabled { get; set; }

        int IndexShards { get; set; }

        int IndexReplicas { get; set; }

        int IndexCopyCount { get; set; }
    }
}