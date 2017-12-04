using System;
using System.Diagnostics.CodeAnalysis;
using System.Web.Mvc;
using Owin;
using SFA.DAS.NLog.Logger;

namespace SFA.DAS.Support.Portal.Web
{
    //[ExcludeFromCodeCoverage]
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledExceptionEventHandler;
        }

        private static void CurrentDomain_UnhandledExceptionEventHandler(object sender, UnhandledExceptionEventArgs e)
        {
            var ex = (Exception)e.ExceptionObject;
            var logger = DependencyResolver.Current.GetService<ILog>();

            logger.Error(ex, "App_Error");
        }
    }
}
