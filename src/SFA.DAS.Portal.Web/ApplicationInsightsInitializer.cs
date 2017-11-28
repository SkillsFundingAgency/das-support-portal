using Microsoft.ApplicationInsights.Channel;

namespace SFA.DAS.Portal.Web
{
    public sealed class ApplicationInsightsInitializer : Microsoft.ApplicationInsights.Extensibility.ITelemetryInitializer
    {
        public void Initialize(ITelemetry telemetry)
        {
            telemetry.Context.Properties["Application"] = "Sfa.Das.Console.Web";
        }
    }
}