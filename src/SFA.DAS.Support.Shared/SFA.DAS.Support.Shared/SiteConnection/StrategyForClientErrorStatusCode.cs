using System;
using System.Net;
using System.Net.Http;
using SFA.DAS.NLog.Logger;

namespace SFA.DAS.Support.Shared.SiteConnection
{
    public class StrategyForClientErrorStatusCode : IHttpStatusCodeStrategy
    {
        private readonly ILog _logger;
        public StrategyForClientErrorStatusCode(ILog logger)
        {
            _logger = logger;
        }
        public HttpStatusCode Low { get; set; } = HttpStatusCode.BadRequest;
        public HttpStatusCode High { get; set; } = HttpStatusCode.UpgradeRequired;
        public HttpStatusCode[] ExcludeStatuses { get; set; } = new HttpStatusCode[] { };

        public HttpStatusCodeDecision Handle(HttpClient client, HttpStatusCode status)
        {
            switch (status)
            {
                case HttpStatusCode.Unauthorized:
                    _logger.Warn( $"Http Status Code ({(int)status}) {status} returned from Site Connector Request, changing token for retry");
                    client.DefaultRequestHeaders.Authorization = null;
                    return HttpStatusCodeDecision.ReturnNull;
                case HttpStatusCode.Forbidden:
                    _logger.Warn( $"Http Status Code ({(int)status}) {status} returned from Site Connector Request");
                    return HttpStatusCodeDecision.ReturnNull;
                case HttpStatusCode.NotFound:
                    _logger.Warn( $"Http Status Code ({(int)status}) {status} returned from Site Connector Request");
                    return HttpStatusCodeDecision.ReturnNull;
                default:
                    _logger.Error( new Exception("Enforced Exeption after invalid Site Response"), $"Http Status Code ({(int)status}) {status} returned from Site Connector Request");
                    return HttpStatusCodeDecision.ReturnNull;
            }
           
        }
    }
}