using ESFA.DAS.Support.Indexer.ApplicationServices.Settings;
using StructureMap.Configuration.DSL;

namespace ESFA.DAS.Support.Indexer.Worker.DependencyResolution
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