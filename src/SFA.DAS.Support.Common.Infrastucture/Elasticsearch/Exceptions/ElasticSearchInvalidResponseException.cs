using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFA.DAS.Support.Common.Infrastucture.Elasticsearch.Exceptions
{
    public class ElasticSearchInvalidResponseException:Exception
    {

        public ElasticSearchInvalidResponseException(int? statusCode)
            :base($"Received non-200 response when trying to fetch the search items from elastic serach Index, Status Code:{statusCode.GetValueOrDefault()}")
        {

        }

    }
}
