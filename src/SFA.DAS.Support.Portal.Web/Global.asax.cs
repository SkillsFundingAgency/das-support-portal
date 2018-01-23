using System;
using System.Diagnostics.CodeAnalysis;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Microsoft.ApplicationInsights.Extensibility;
using SFA.DAS.NLog.Logger;

namespace SFA.DAS.Support.Portal.Web
{
    [ExcludeFromCodeCoverage]
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            MvcHandler.DisableMvcResponseHeader = true;
            var logger = DependencyResolver.Current.GetService<ILog>();

            logger.Info("Starting Web Role");

            SetupApplicationInsights();

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            ControllerFilters.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            logger.Info("Web Role started");
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            var ex = Server.GetLastError().GetBaseException();
            var logger = DependencyResolver.Current.GetService<ILog>();
            logger.Error(ex, "App_Error");
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

            var logger = DependencyResolver.Current.GetService<ILog>();
            logger.Info($"{context.Request.HttpMethod} {context.Request.Path}");
        }

        private void SetupApplicationInsights()
        {
            TelemetryConfiguration.Active.InstrumentationKey =
                WebConfigurationManager.AppSettings["InstrumentationKey"];

            TelemetryConfiguration.Active.TelemetryInitializers.Add(new ApplicationInsightsInitializer());
        }
    }
}