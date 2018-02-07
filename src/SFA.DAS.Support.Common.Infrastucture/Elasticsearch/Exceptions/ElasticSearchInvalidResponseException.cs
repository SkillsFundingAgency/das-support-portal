using System;

namespace SFA.DAS.Support.Common.Infrastucture.Elasticsearch.Exceptions
{
    public class ElasticSearchInvalidResponseException : Exception
    {
        public ElasticSearchInvalidResponseException(string msg, Exception exception)
            : base(msg, exception)
        {
        }
    }
}