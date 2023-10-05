using System.Diagnostics.CodeAnalysis;
using MediatR;
using StructureMap.Configuration.DSL;

namespace SFA.DAS.Support.Portal.Web.DependencyResolution
{
    [ExcludeFromCodeCoverage]
    public class MediatrRegistry : Registry
    {
        public MediatrRegistry()
        {
            For<IMediator>().Use<Mediator>();
        }
    }
}