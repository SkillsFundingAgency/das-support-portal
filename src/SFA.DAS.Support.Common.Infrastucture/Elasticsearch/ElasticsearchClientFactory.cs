using System.Linq;
using Elasticsearch.Net;
using Nest;
using SFA.DAS.Support.Common.Infrastucture.Extensions;
using SFA.DAS.Support.Common.Infrastucture.Settings;

namespace SFA.DAS.Support.Common.Infrastucture.Elasticsearch
{
    public class ElasticsearchClientFactory : IElasticsearchClientFactory
    {
        private readonly ISearchSettings _indexerSettings;

        public ElasticsearchClientFactory(ISearchSettings indexerSettings)
        {
            _indexerSettings = indexerSettings;
        }

        public IElasticClient GetElasticClient()
        {
            var indexerSettingsElasticServerUrls = _indexerSettings.ElasticServerUrls.ToList();

            if (_indexerSettings.IgnoreSslCertificateEnabled)
            {
                using (var settings = new ConnectionSettings(
                    new StaticConnectionPool(indexerSettingsElasticServerUrls),
                    new MyCertificateIgnoringHttpConnection()))
                {
                    SetDefaultSettings(settings);

                    return new ElasticClient(settings);
                }
            }

            using (var settings = new ConnectionSettings(new StaticConnectionPool(indexerSettingsElasticServerUrls)))
            {
                SetDefaultSettings(settings);

                return new ElasticClient(settings);
            }
        }

        private void SetDefaultSettings(ConnectionSettings settings)
        {
            if (_indexerSettings.Elk5Enabled)
            {
                settings.BasicAuthentication(_indexerSettings.ElasticUsername, _indexerSettings.ElasticPassword);
            }
        }
    }
}