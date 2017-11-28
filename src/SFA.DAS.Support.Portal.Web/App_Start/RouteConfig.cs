using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace SFA.DAS.Support.Portal.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.LowercaseUrls = true;
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //enable attribute routing
            try
            {
                routes.MapMvcAttributeRoutes();
            }
            catch 
            {
                // not valid scenario
            }

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
