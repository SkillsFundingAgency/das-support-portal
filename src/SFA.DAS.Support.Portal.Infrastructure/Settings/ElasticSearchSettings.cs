using System;
using System.Collections.Generic;
using SFA.DAS.Support.Common.Infrastucture.Settings;
using SFA.DAS.Support.Portal.Core.Services;

namespace SFA.DAS.Support.Indexer.ApplicationServices.Settings
{
    public class ElasticSearchSettings : ISearchSettings
    {
        private readonly IProvideSettings _settings;

        public ElasticSearchSettings(IProvideSettings settings)
        {
            _settings = settings;
        }

        public string AdminApiKey => _settings.GetSetting("Support:Azure:Search:AdminKey");
        public string ServiceName => _settings.GetSetting("Support:Azure:Search:ServiceName");


        public string IndexName => string.Format(_settings.GetSetting("IndexNameFormat"), _settings.GetSetting("EnvironmentName"));

        public IEnumerable<Uri> ElasticServerUrls
        {
            get
            {
                if (!Uri.TryCreate(_settings.GetSetting("Support:Elastic:Search:Url"), UriKind.Absolute, out Uri url))
                {
                    throw new ArgumentException($"invalid {nameof(ElasticServerUrls)} parameter specified in settings");
                }

                return new List<Uri> { url };

            }
        }

        public string ElasticUsername => _settings.GetSetting("Support:Elastic:Search:Username");

        public string ElasticPassword => _settings.GetSetting("Support:Elastic:Search:Password");

        public bool Elk5Enabled
        {
            get
            {
                if (!bool.TryParse(_settings.GetSetting("Support:Elastic:Search:Elk5Enabled"), out bool elk5Enabled))
                {
                    throw new ArgumentException($"invalid {nameof(Elk5Enabled)} parameter specified in settings");
                }

                return elk5Enabled;
            }
        }

        public bool IgnoreSslCertificateEnabled
        {
            get
            {
                if (!bool.TryParse(_settings.GetSetting("Support:Elastic:Search:IgnoreSslCertificateEnabled"), out bool ignoreSslCertificateEnabled))
                {
                    throw new ArgumentException($"invalid {nameof(ignoreSslCertificateEnabled)} parameter specified in settings");
                }

                return ignoreSslCertificateEnabled;
            }
        }

        public int IndexShards
        {
            get
            {
                if (!int.TryParse(_settings.GetSetting("Support:Elastic:Search:IndexShards"), out int indexShards))
                {
                    throw new ArgumentException($"invalid {nameof(IndexShards)} parameter specified in settings");
                }

                return indexShards;
            }
        }
       
        public int IndexReplicas
        {
            get
            {
                if (!int.TryParse(_settings.GetSetting("Support:Elastic:Search:IndexReplicas"), out int indexReplicas))
                {
                    throw new ArgumentException($"invalid {nameof(IndexReplicas)} parameter specified in settings");
                }

                return indexReplicas;
            }
        }

    }
}