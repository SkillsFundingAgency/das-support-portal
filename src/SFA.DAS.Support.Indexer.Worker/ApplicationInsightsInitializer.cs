using System.Diagnostics.CodeAnalysis;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;

namespace SFA.DAS.Support.Indexer.Worker
{
    [ExcludeFromCodeCoverage]
    public sealed class ApplicationInsightsInitializer : ITelemetryInitializer
    {
        public void Initialize(ITelemetry telemetry)
        {
            telemetry.Context.Properties["Application"] = "SFA.DAS.Support.Indexer.Worker";
        }
    }
}