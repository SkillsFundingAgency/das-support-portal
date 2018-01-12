using System.Linq;
using System.Net;
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
            ServicePointManager.SecurityProtocol =
                                                   SecurityProtocolType.Tls12 
                ; /*
                                                     SecurityProtocolType.Ssl3 
                                                   | SecurityProtocolType.Tls 
                                                   | SecurityProtocolType.Tls11
                                                   */
            var connectionPool = new SingleNodeConnectionPool(indexerSettingsElasticServerUrls.First());
            ConnectionSettings settings = null;
            if (_indexerSettings.IgnoreSslCertificateEnabled)
            {
                var myCertificateIgnoringHttpConnection = new MyCertificateIgnoringHttpConnection();

                settings = new ConnectionSettings(
                   connectionPool,
                   myCertificateIgnoringHttpConnection);

                SetDefaultSettings(settings);

                return new ElasticClient(settings);

            }

            settings = new ConnectionSettings(connectionPool);

            SetDefaultSettings(settings);

            return new ElasticClient(settings);

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