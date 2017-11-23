using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ESFA.DAS.Support.Indexer.Infrastructure.Extensions
{
    public static class HttpClientExtensions
    {
        public static Task<T> DownloadAs<T>(this HttpClient client, Uri uri)
        {
            // I know I am firing this on another thread
            // to keep UI free from any smallest task like
            // preparing httpclient, setting headers
            // checking for result or anything..., why do that on
            // UI Thread?

            // this helps us using excessive logging required for
            // debugging and diagnostics

            return Task.Run(async () =>
            {
                // following parses url and makes sure
                // it is a valid url
                // there is no need for this to be done on 
                // UI thread
                var request = new HttpRequestMessage(HttpMethod.Get, uri);

                // do some checks, set some headers...
                // secrete code !!!

                var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);

                var content = await response.Content.ReadAsStringAsync();

                if ((int) response.StatusCode > 300)
                    throw new InvalidOperationException(response.ReasonPhrase
                                                        + "\r\n" + content);
                return JsonConvert.DeserializeObject<T>(content);
            });
        }
    }
}