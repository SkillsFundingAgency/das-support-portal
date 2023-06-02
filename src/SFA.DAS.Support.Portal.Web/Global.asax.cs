using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Microsoft.ApplicationInsights.Extensibility;
using SFA.DAS.NLog.Logger;
using SFA.DAS.Web.Policy;

namespace SFA.DAS.Support.Portal.Web
{
    [ExcludeFromCodeCoverage]
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            MvcHandler.DisableMvcResponseHeader = true;
            var logger = DependencyResolver.Current.GetService<ILog>() ?? new NLogLogger();

            logger.Info("Starting Web Role");

            SetupApplicationInsights();

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            ControllerFilters.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            logger.Info("Web Role started");
        }

        protected void Application_PreSendRequestHeaders(object sender, EventArgs e)
        {
            if (HttpContext.Current == null) return;
            new HttpContextPolicyProvider(
                new List<IHttpContextPolicy>
                {
                    new ResponseHeaderRestrictionPolicy()
                }
            ).Apply(new HttpContextWrapper(HttpContext.Current), PolicyConcern.HttpResponse);
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            var ex = Server.GetLastError().GetBaseException();
            BuildAndLogExceptionReport(ex);
        }

        private void BuildAndLogExceptionReport(Exception ex)
        {
            var logger = DependencyResolver.Current.GetService<ILog>() ?? new NLogLogger();

            var exceptionReport = $"An Unhandled exception was caught by {nameof(Application_Error)}\r\n";
            exceptionReport += TryAddUserContext();
            exceptionReport += TryAddHttpRequestContext();
            exceptionReport += "\r\nException Stack Trace follows:\r\n\r\n";
            logger.Error(ex, exceptionReport);
        }

        private string TryAddHttpRequestContext()
        {
            try
            {
                return
                    $"Host Address: {HttpContext.Current?.Request?.UserHostAddress ?? "Unknown"}\r\nHttp Method: {HttpContext.Current?.Request?.HttpMethod ?? "Unknown"}\r\nRawUrl: {HttpContext.Current?.Request?.RawUrl ?? "Unknown"}";
            }
            catch (Exception)
            {
                return "The HttpRequest is not available in this context.";
            }
        }


        private string TryAddUserContext()
        {
            return $"User Name: {HttpContext.Current?.User?.Identity?.Name ?? "Unknown"}\r\n";
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            var context = Context;
            var application = sender as HttpApplication;

            application?.Context?.Response.Headers.Remove("Server");
            application?.Context?.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            application?.Context?.Response.Cache.AppendCacheExtension("no-store, must-revalidate");
            application?.Context?.Response.AppendHeader("Pragma", "no-cache");
            application?.Context?.Response.AppendHeader("Expires", "0");

            if (context.Request.Path.StartsWith("/__browserlink")) return;

            var logger = DependencyResolver.Current.GetService<ILog>() ?? new NLogLogger();
            logger.Info($"{context.Request.HttpMethod} {context.Request.Path}");
        }

        private void SetupApplicationInsights()
        {
            TelemetryConfiguration.Active.InstrumentationKey =
                WebConfigurationManager.AppSettings["APPINSIGHTS_INSTRUMENTATIONKEY"];

            TelemetryConfiguration.Active.TelemetryInitializers.Add(new ApplicationInsightsInitializer());
        }
    }
}
