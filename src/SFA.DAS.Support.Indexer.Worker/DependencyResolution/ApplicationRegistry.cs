using SFA.DAS.Support.Common.Infrastucture.Settings;
using SFA.DAS.Support.Indexer.ApplicationServices.Settings;
using StructureMap.Configuration.DSL;

namespace SFA.DAS.Support.Indexer.Worker.DependencyResolution
{
    public class ApplicationRegistry : Registry
    {
        public ApplicationRegistry()
        {
            For<ISiteSettings>().Use<SiteSettings>();
            For<ISearchSettings>().Use<SearchSettings>();
        }
    }
}