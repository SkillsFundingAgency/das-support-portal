using System.Net;
using System.Net.Http;
using SFA.DAS.NLog.Logger;

namespace SFA.DAS.Support.Shared.SiteConnection
{
    public class StrategyForSystemErrorStatusCode : IHttpStatusCodeStrategy
    {
        private readonly ILog _logger;

        public StrategyForSystemErrorStatusCode(ILog logger)
        {
            _logger = logger;
        }

        public HttpStatusCode Low { get; set; } = HttpStatusCode.InternalServerError;
        public HttpStatusCode High { get; set; } = HttpStatusCode.HttpVersionNotSupported;
        public HttpStatusCode[] ExcludeStatuses { get; set; } = { };

        public HttpStatusCodeDecision Handle(HttpClient client, HttpStatusCode status)
        {
            _logger.Info($"Http Status Code ({(int) status}) {status} returned from Site Connector Request");
            return HttpStatusCodeDecision.HandleException;
        }
    }
}