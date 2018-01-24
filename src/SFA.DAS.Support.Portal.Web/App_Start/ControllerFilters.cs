using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Web.Mvc;
using Microsoft.Azure;
using SFA.DAS.Support.Portal.Core.Services;

namespace SFA.DAS.Support.Portal.Web
{
    [ExcludeFromCodeCoverage]
    public class ControllerFilters
    {
        private static readonly string AdfsMetadata = CloudConfigurationManager.GetSetting("ida_ADFSMetadata");

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            var roleSettings = DependencyResolver.Current.GetService<IRoleSettings>();
            if (!Debugger.IsAttached && !string.IsNullOrWhiteSpace(AdfsMetadata))
                filters.Add(new AuthorizeAttribute {Roles = roleSettings.ConsoleUserRole});
        }
    }
}