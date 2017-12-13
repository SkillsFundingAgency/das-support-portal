using NUnit.Framework;
using SFA.DAS.Support.Portal.ApplicationServices.Responses;
using SFA.DAS.Support.Portal.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFA.DAS.Support.Portal.UnitTests.Web.Services
{
    [TestFixture]
    public class SearchTableResultBuilderTest
    {


        public void GivenValidResponseShouldGenerateResultTable()
        {
            // Arrange

            var searchResponse = new SearchResponse();
            //{
            //    Results = new Dictionary<string, List<string>>()
            //    {
            //        {
            //            "USER",
            //            new List<string>
            //            {
            //                "{ 
            //                  'Id': null,
            //                  'FirstName': "",
            //                  'LastName': "",
            //                  'IsActive': "",
            //                  'IsLocked': "",
            //                  'Email': "",
            //                  'Href': ""
            //                }"
            //           }
            //        }

            //    }
            //};



            var sut = new SearchTableResultBuilder();
            var table = sut.CreateTableResult(searchResponse);

        }


    }
}
