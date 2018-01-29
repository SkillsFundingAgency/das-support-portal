using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFA.DAS.Support.Common.Infrastucture.Elasticsearch.Exceptions
{
    public class ElasticSearchInvalidResponseException:Exception
    {

        public ElasticSearchInvalidResponseException(int? statusCode, string errorReason, Exception exception)
            :base($"Received non-200 response Status Code:{statusCode.GetValueOrDefault()} Reason: {errorReason}", exception)
        {

        }

    }
}
