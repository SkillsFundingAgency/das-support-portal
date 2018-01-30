using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFA.DAS.Support.Common.Infrastucture.Elasticsearch.Exceptions
{
    public class ElasticSearchInvalidResponseException:Exception
    {

        public ElasticSearchInvalidResponseException(string msg, Exception exception )
            :base(msg, exception)
        {

        }

    }
}
