using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace SFA.DAS.Support.Shared.SiteConnection
{
    public interface ISiteConnector
    {
        Task<string> Download(string url);
        Task<T> Download<T>(Uri uri) where T : class;
        Task<T> Download<T>(string url) where T : class;
        Task<T> Upload<T>(Uri uri, IDictionary<string, string> formData) where T : class;
        Exception LastException { get; set; }
        HttpStatusCode LastCode { get; set; }
    }
}