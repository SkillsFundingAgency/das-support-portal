using System;
using System.Net;

namespace SFA.DAS.Support.Portal.ApplicationServices.Models
{
    public class ResourceResultModel
    {
        public string Resource { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public Exception Exception { get; set; }
    }
}