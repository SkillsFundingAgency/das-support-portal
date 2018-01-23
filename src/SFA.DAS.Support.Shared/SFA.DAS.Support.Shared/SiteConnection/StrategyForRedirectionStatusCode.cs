using System;
using System.Net;
using System.Net.Http;
using SFA.DAS.NLog.Logger;

namespace SFA.DAS.Support.Shared.SiteConnection
{
    public class StrategyForRedirectionStatusCode : IHttpStatusCodeStrategy
    {
        private readonly ILog _logger;

        public StrategyForRedirectionStatusCode(ILog logger)
        {
            _logger = logger;
        }
        public HttpStatusCode Low { get; set; } = HttpStatusCode.Ambiguous;
        public HttpStatusCode High { get; set; } = HttpStatusCode.RedirectKeepVerb;
        public HttpStatusCode[] ExcludeStatuses { get; set; } = new HttpStatusCode[]{};

        public HttpStatusCodeDecision Handle(HttpClient client, HttpStatusCode status)
        {
            _logger.Info($"Http Status Code ({(int)status}) {status} returned from Site Connector Request");
            return HttpStatusCodeDecision.ReturnNull;
        }
    }
}