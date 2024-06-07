using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace SFA.DAS.Support.Shared.SiteConnection
{
    public interface ISiteConnector
    {
        string LastContent { get; set; }
        Exception LastException { get; set; }
        HttpStatusCode LastCode { get; set; }
        HttpStatusCodeDecision HttpStatusCodeDecision { get; set; }

        Task<string> Download(Uri url, string resourceIdentity);

        Task<T> Download<T>(Uri uri, string resourceIdentity) where T : class;

        Task<T> Download<T>(string url, string resourceIdentity) where T : class;

        Task<T> Upload<T>(Uri uri, IDictionary<string, string> formData, string resourceIdentity) where T : class;

        Task<T> Upload<T>(Uri uri, string content, string resourceIdentity, bool isJsonContent = false) where T : class;
        Task Upload(Uri uri, string content, string resourceIdentity);
    }
}