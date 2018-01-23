using System;
using System.Net;
using System.Net.Http;
using SFA.DAS.NLog.Logger;

namespace SFA.DAS.Support.Shared.SiteConnection
{
    public class StrategyForInformationStatusCode : IHttpStatusCodeStrategy
    {
       
        private readonly ILog _logger;

        public StrategyForInformationStatusCode(ILog logger)
        {
            _logger = logger;
        }

        public HttpStatusCode Low { get; set; } = HttpStatusCode.Continue;
        public HttpStatusCode High { get; set; } = HttpStatusCode.SwitchingProtocols;
        public HttpStatusCode[] ExcludeStatuses { get; set; } = new HttpStatusCode[]{};
        public HttpStatusCodeDecision Handle(HttpClient client, HttpStatusCode status)
        {
            _logger.Info($"Http Status Code ({(int)status}) {status} from Site Connector Request");
            return HttpStatusCodeDecision.ReturnNull;
        }
    }
}