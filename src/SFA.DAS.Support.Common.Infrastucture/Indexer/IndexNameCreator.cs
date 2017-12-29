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

        public string CreateIndexesToDeleteName(string IndexNameFormat, string environment, SearchCategory searchCategory)
        {
            return CreateDerivedIndexName(IndexNameFormat, environment, searchCategory, DateTime.UtcNow.AddDays(-1), "yyyy-MMM-dd");
        }

        public string CreateNewIndexName(string IndexNameFormat, string environment, SearchCategory searchCategory)
        {
          return CreateDerivedIndexName(IndexNameFormat, environment, searchCategory, DateTime.UtcNow, "yyyy-MMM-dd-HH-mm");
        }

        private string CreateDerivedIndexName(string IndexNameFormat, string environmentName, SearchCategory searchCategory, DateTime date, string dateFormat)
        {
            var aliasName = CreateIndexesAliasName(IndexNameFormat, environmentName, searchCategory);
            return $"{aliasName}_{date.ToString(dateFormat)}".ToLower();
        }

    }
}
