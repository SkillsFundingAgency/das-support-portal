using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using SFA.DAS.NLog.Logger;
using SFA.DAS.Support.Portal.ApplicationServices.Settings;
using SFA.DAS.Support.Shared;
using SFA.DAS.Support.Shared.Discovery;
using SFA.DAS.Support.Shared.SiteConnection;

namespace SFA.DAS.Support.Portal.Web.Controllers
{
    [System.Web.Mvc.RoutePrefix("api/status")]
    public class StatusController : ApiController
    {
        private ISiteConnector _siteConnector;
        private readonly ISubSiteConnectorSettings _siteSettings;
        private readonly ILog _log;

        public StatusController(ISiteConnector siteConnector, ISubSiteConnectorSettings siteSettings, ILog log)
        {
            _siteConnector = siteConnector;
            _siteSettings = siteSettings;
            _log = log;
        }

        [System.Web.Mvc.AllowAnonymous]
        public async Task<IHttpActionResult> Get()
        {
            // Get the status of each site
            // add it to this one and output the results
            var sites = _siteSettings.SubSiteConnectorSettings.Select(x => $"{x.Key}|{x.BaseUrl}").ToList();
            var localResult = new
            {
                ServiceName = AddServiceName(),
                ServiceVersion = AddServiceVersion(),
                ServiceTime = AddServerTime(),
                Request = AddRequestContext(),
                SubSites = new Dictionary<SupportServiceIdentity, dynamic>(),
                Sites = sites
            };

            try
            {
                foreach (var subSite in _siteSettings.SubSiteConnectorSettings)
                {
                    var uri = new Uri(new Uri(subSite.BaseUrl), "api/status");
                    SupportServiceIdentity resourceKey;
                    if (!Enum.TryParse<SupportServiceIdentity>(subSite.Key, out resourceKey))
                    {
                        _log.Warn($"Error while trying to convert subSite {subSite.Key} to SupportServiceIdentity enum ");
                        continue;
                    }

                    try
                    {
                        await _siteConnector.Download<dynamic>(uri, subSite.IdentifierUri);

                        localResult.SubSites.Add(resourceKey, new
                        {
                            Result = _siteConnector.HttpStatusCodeDecision,
                            HttpStatusCode = _siteConnector.LastCode,
                            Exception = _siteConnector.LastException,
                            Content = _siteConnector.LastContent,
                        });
                    }
                    catch (Exception e)
                    {
                        localResult.SubSites.Add(resourceKey, new
                        {
                            Result = _siteConnector.HttpStatusCodeDecision,
                            HttpStatusCode = _siteConnector.LastCode,
                            Exception = e,
                            Content = _siteConnector.LastContent,
                        });
                    }
                }
            }
            catch
            {
                // ignored
            }

            return Ok(localResult);
        }

        private string AddServiceVersion()
        {
            try
            {
                return Assembly.GetExecutingAssembly().Version();
            }
            catch (Exception)
            {
                return "Unknown";
            }
        }

        private string AddRequestContext()
        {
            try
            {
                return $" {HttpContext.Current.Request.HttpMethod}: {HttpContext.Current.Request.RawUrl}";
            }
            catch
            {
                return "Unknown";
            }
        }

        private DateTimeOffset AddServerTime()
        {
            return DateTimeOffset.UtcNow;
        }

        private string AddServiceName()
        {
            try
            {
                return Assembly.GetExecutingAssembly().Title();
            }
            catch
            {
                return "Unknown";
            }
        }
    }
}