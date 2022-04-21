using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
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
        private readonly ISiteSettings _siteSettings;

        public StatusController(ISiteConnector siteConnector, ISiteSettings siteSettings)
        {
            _siteConnector = siteConnector;
            _siteSettings = siteSettings;
        }
        [System.Web.Mvc.AllowAnonymous]
        public async Task<IHttpActionResult> Get()
        {
            // Get the status of each site
            // add it to this one and output the results
            var sites = _siteSettings.BaseUrls.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
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
                var subSites = new Dictionary<SupportServiceIdentity, Uri>();
                foreach (var subSite in sites)
                {
                    var siteElements = subSite.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                    if (siteElements.Length != 2) continue;
                    if (string.IsNullOrWhiteSpace(siteElements[0])) continue;
                    if (string.IsNullOrWhiteSpace(siteElements[1])) continue;
                    subSites.Add((SupportServiceIdentity)Enum.Parse(typeof(SupportServiceIdentity), siteElements[0]), new Uri(siteElements[1]));
                }

                foreach (var subSite in subSites)
                {
                    var uri = new Uri(subSite.Value, "api/status");
                    try
                    {
                        await _siteConnector.Download<dynamic>(uri, SupportServiceResourceKey.EmployerAccount); //TODO: need to implement generic SupportServiceResourceKey
                        localResult.SubSites.Add(subSite.Key, new
                        {
                            Result = _siteConnector.HttpStatusCodeDecision,
                            HttpStatusCode = _siteConnector.LastCode,
                            Exception = _siteConnector.LastException,
                            Content = _siteConnector.LastContent,
                        });
                    }
                    catch (Exception e)
                    {
                        localResult.SubSites.Add(subSite.Key, new
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