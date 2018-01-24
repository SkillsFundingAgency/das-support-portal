using System.Net;
using System.Net.Http;
using SFA.DAS.NLog.Logger;

namespace SFA.DAS.Support.Shared.SiteConnection
{
    public class StrategyForSuccessStatusCode : IHttpStatusCodeStrategy
    {
        private readonly ILog _logger;

        public StrategyForSuccessStatusCode(ILog logger)
        {
            _logger = logger;
        }

        public HttpStatusCode Low { get; set; } = HttpStatusCode.OK;
        public HttpStatusCode High { get; set; } = HttpStatusCode.PartialContent;
        public HttpStatusCode[] ExcludeStatuses { get; set; } = { };

        public HttpStatusCodeDecision Handle(HttpClient client, HttpStatusCode status)
        {
            _logger.Info($"Http Status Code ({(int) status}) {status} returned from Site Connector Request");
            return HttpStatusCodeDecision.Continue;
        }
    }
}