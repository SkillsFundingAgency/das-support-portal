using System;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.Support.Common.Infrastucture.Elasticsearch.Exceptions
{
    [ExcludeFromCodeCoverage]
    public class ElasticSearchInvalidResponseException : Exception
    {
        public ElasticSearchInvalidResponseException(string msg, Exception exception)
            : base(msg, exception)
        {
        }
    }
}