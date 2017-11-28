using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace SFA.DAS.Support.Portal.ApplicationServices.Services
{
    public interface IDownload
    {
        Task<string> Download(string url);
        Task<T> Download<T>(Uri uri) where T : class;
        Task<T> Download<T>(string url) where T : class;
        Task<HttpResponseMessage> Post(Uri uri, IDictionary<string, string> formData);
    }
}
