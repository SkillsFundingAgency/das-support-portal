using System;
using System.Net;
using System.Net.Http;

namespace SFA.DAS.Support.Shared.SiteConnection
{
    public interface IHttpStatusCodeStrategy
    {
        HttpStatusCode Low { get; set; }
        HttpStatusCode High { get; set; }

        HttpStatusCode[] ExcludeStatuses { get; set; }

        HttpStatusCodeDecision Handle(HttpClient client, HttpStatusCode status);
    }

}
