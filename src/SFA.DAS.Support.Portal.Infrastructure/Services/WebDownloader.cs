using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using SFA.DAS.Support.Portal.ApplicationServices.Services;
using SFA.DAS.Support.Portal.Infrastructure.Extensions;

namespace SFA.DAS.Support.Portal.Infrastructure.Services
{
    public class WebDownloader : IDownload
    {
        private HttpClient _client;

        public WebDownloader()
        {
            _client = new HttpClient();
        }

        public Task<T> Download<T>(string url) where T : class
        {
            return Download<T>(new Uri(url));
        }

        public Task<HttpResponseMessage> Post(Uri uri, IDictionary<string, string> formData)
        {
            return _client.PostAsync(uri, new FormUrlEncodedContent(formData));
        }

        public Task<string> Download(string url)
        {
            return _client.Download(new Uri(url));
        }

        public Task<T> Download<T>(Uri uri) where T : class
        {
            return _client.DownloadAs<T>(uri);
        }
    }
}
