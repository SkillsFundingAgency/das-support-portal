using System.Diagnostics.CodeAnalysis;
using Microsoft.ApplicationInsights.Channel;

namespace SFA.DAS.Support.Portal.Web
{
    //[ExcludeFromCodeCoverage]
    public sealed class ApplicationInsightsInitializer : Microsoft.ApplicationInsights.Extensibility.ITelemetryInitializer
    {
        public void Initialize(ITelemetry telemetry)
        {
            telemetry.Context.Properties["Application"] = "SFA.DAS.Support.Portal.Web";
        }
    }
}