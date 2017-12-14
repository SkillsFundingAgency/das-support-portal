using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using SFA.DAS.Support.Common.Infrastucture.Settings;
using SFA.DAS.Support.Indexer.Core.Services;
using System.Linq;

namespace SFA.DAS.Support.Indexer.ApplicationServices.Settings
{
    [ExcludeFromCodeCoverage]
    public class SearchSettings : ISearchSettings
    {
        private readonly IProvideSettings _settings;

        public SearchSettings(IProvideSettings settings)
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
                var urls = _settings.GetSetting("Support:Elastic:Search:Url")?.Split(Convert.ToChar(","));

                if(urls == null || urls.Length == 0)
                {
                    throw new ArgumentException($"invalid {nameof(ElasticServerUrls)} parameter specified in settings");
                }

                var elasticSearchUrls = new List<Uri>();

                foreach (var url in urls)
                {
                    if (Uri.TryCreate(url, UriKind.Absolute, out Uri serverUrl))
                    {
                        elasticSearchUrls.Add(serverUrl);
                    }
                }

                if (!elasticSearchUrls.Any())
                {
                    throw new ArgumentException($"invalid {nameof(ElasticServerUrls)} parameter specified in settings");
                }

                return elasticSearchUrls;

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