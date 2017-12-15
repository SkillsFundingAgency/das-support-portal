using Newtonsoft.Json;
using SFA.DAS.Support.Portal.ApplicationServices.Responses;
using SFA.DAS.Support.Portal.Web.ViewModels;
using SFA.DAS.Support.Shared;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;

namespace SFA.DAS.Support.Portal.Web.Services
{
    public class SearchTableResultBuilder : ISearchTableResultBuilder
    {
        public SearchTableResultViewModel CreateTableResult(SearchResponse model)
        {
            var result = new SearchTableResultViewModel();

            if (model.SearchResultsMetadata.Any() && model.Results.Any())
            {
                var resultCategory = model.Results.Keys.FirstOrDefault();
                var resultJsonStrings = model.Results.FirstOrDefault(o => o.Key == resultCategory);

                var metaData = model.SearchResultsMetadata.FirstOrDefault(x => x.SearchResultCategory == resultCategory);
                var columnsToshow = metaData?.ColumnDefinitions.Where(x => !x.HideColumn);


                foreach (var columnDef in columnsToshow)
                {
                    result.Columns.Add(columnDef.Name);
                }

                result.Rows = GetResultRows(columnsToshow, resultJsonStrings.Value);
            }

            return result;

        }

        private List<ResultRow> GetResultRows(IEnumerable<SearchColumnDefinition> columnMetadata, List<string> jsonStrings)
        {
            var rows = new List<ResultRow>();

            foreach (var resultJsonString in jsonStrings)
            {
                var row = new ResultRow();

                foreach (var columnDef in columnMetadata)
                {

                    var resultObject = JsonConvert.DeserializeObject<ExpandoObject>(resultJsonString);
                    var columnCellText = resultObject.FirstOrDefault(o => o.Key.Equals(columnDef.Name, StringComparison.OrdinalIgnoreCase)).Value?.ToString();
                    var linkUrl = string.Empty;

                    if (columnDef.Link != null)
                    {
                        var linkColumnValue = resultObject.FirstOrDefault(o => o.Key.Equals(columnDef.Link.MapColumnName, StringComparison.OrdinalIgnoreCase)).Value.ToString();
                        linkUrl = string.Format(columnDef.Link.Format, linkColumnValue);
                    }

                    var cell = new ResultCell
                    {
                        Value = columnCellText,
                        LinkUrl = linkUrl
                    };

                    row.CellValues.Add(cell);
                }

                rows.Add(row);
            }

            return rows;
        }

    }
}