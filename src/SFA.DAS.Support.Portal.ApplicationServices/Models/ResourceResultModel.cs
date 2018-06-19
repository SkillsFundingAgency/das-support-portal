using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace SFA.DAS.Support.Portal.ApplicationServices.Models
{
    [ExcludeFromCodeCoverage]
    public class ResourceResultModel
    {
        public string Resource { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public Exception Exception { get; set; }
    }
}