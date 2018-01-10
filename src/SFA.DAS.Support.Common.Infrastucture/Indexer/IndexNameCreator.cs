using System;
using SFA.DAS.Support.Shared.SearchIndexModel;

namespace SFA.DAS.Support.Common.Infrastucture.Indexer
{
    public class IndexNameCreator : IIndexNameCreator
    {
        public string CreateIndexesAliasName(string indexName, SearchCategory searchCategory)
        {
            var indexType = Enum.GetName(typeof(SearchCategory), searchCategory);
            return $"{indexName}-{indexType}".ToLower();
        }

        public string CreateNewIndexName(string indexName, SearchCategory searchCategory)
        {
            var aliasName = CreateIndexesAliasName(indexName, searchCategory);
            return $"{aliasName}_{DateTime.UtcNow.ToString("yyyyMMddHHmmss")}".ToLower();
        }

    }
}
