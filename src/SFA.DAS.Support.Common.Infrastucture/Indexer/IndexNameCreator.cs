using System;
using SFA.DAS.Support.Shared.SearchIndexModel;

namespace SFA.DAS.Support.Common.Infrastucture.Indexer
{
    public class IndexNameCreator : IIndexNameCreator
    {
        public string CreateIndexesAliasName(string IndexNameFormat, string environment, SearchCategory searchCategory)
        {
            var indexType = Enum.GetName(typeof(SearchCategory), searchCategory);
            return $"{string.Format(IndexNameFormat, environment)}-{indexType}".ToLower();
        }

        public string CreateNewIndexName(string IndexNameFormat, string environment, SearchCategory searchCategory)
        {
            var aliasName = CreateIndexesAliasName(IndexNameFormat, environment, searchCategory);
            return $"{aliasName}_{DateTime.UtcNow.ToString("yyyyMMddHHmmss")}".ToLower();
        }

    }
}
