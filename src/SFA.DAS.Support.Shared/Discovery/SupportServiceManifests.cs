using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.Support.Shared.Discovery
{
    /// <summary>
    ///     The class used to provide run-time knowledge of available services. Deserialised from
    /// </summary>
    [ExcludeFromCodeCoverage]
    public sealed class SupportServiceManifests : Dictionary<SupportServiceIdentity, SiteManifest>
    {
    }
}