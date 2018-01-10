using SFA.DAS.Support.Shared.SearchIndexModel;
namespace SFA.DAS.Support.Common.Infrastucture.Indexer
{
    public interface IIndexNameCreator
    {
        string CreateIndexesAliasName(string indexName, SearchCategory searchCategory);

        string CreateNewIndexName(string indexName, SearchCategory searchCategory);
    }
}
   