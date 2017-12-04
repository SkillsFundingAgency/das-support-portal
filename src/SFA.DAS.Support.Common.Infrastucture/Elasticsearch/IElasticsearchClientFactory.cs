using Nest;

namespace SFA.DAS.Support.Common.Infrastucture.Elasticsearch
{
    public interface IElasticsearchClientFactory
    {
        IElasticClient GetElasticClient();
    }
}