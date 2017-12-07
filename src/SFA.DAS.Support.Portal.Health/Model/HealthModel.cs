using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.Support.Portal.Health.Model
{
    //[ExcludeFromCodeCoverage]
    public class HealthModel
    {
        public string Version { get; set; }

        public string AssemblyVersion { get; set; }

        public Status ApiStatus { get; set; }
    }
}
