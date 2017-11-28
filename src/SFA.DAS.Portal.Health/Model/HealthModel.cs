namespace SFA.DAS.Portal.Health.Model
{
    public class HealthModel
    {
        public string Version { get; set; }

        public string AssemblyVersion { get; set; }

        public Status ApiStatus { get; set; }
    }
}
