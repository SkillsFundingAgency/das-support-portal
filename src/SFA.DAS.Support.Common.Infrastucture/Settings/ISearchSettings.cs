using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFA.DAS.Support.Common.Infrastucture.Settings
{
    public interface ISearchSettings
    {
        string IndexNameFormat { get; }

        IEnumerable<Uri> ElasticServerUrls { get; }

        string ElasticUsername { get; }

        string ElasticPassword { get; }

        bool Elk5Enabled { get; }

        bool IgnoreSslCertificateEnabled { get; }

        int IndexShards { get; }

        int IndexReplicas { get; }


        //----Azure search settings to be removed
        string AdminApiKey { get; }
        string ServiceName { get; }

    }
}
