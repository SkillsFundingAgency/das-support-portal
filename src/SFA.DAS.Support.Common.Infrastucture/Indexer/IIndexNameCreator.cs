using SFA.DAS.Support.Shared.SearchIndexModel;
namespace SFA.DAS.Support.Common.Infrastucture.Indexer
{
    public interface IIndexNameCreator
    {
        string CreateNewIndexName(string IndexNameFormat, string environment, SearchCategory searchCategory);

        string CreateIndexesToDeleteName(string IndexNameFormat, string environment, SearchCategory searchCategory);

        string CreateIndexesAliasName(string IndexNameFormat, string environment, SearchCategory searchCategory);
    }
}
   