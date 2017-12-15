using FluentAssertions;
using Newtonsoft.Json;
using NUnit.Framework;
using SFA.DAS.Support.Portal.ApplicationServices.Responses;
using SFA.DAS.Support.Portal.Web.Services;
using SFA.DAS.Support.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SFA.DAS.Support.Portal.UnitTests.Web.Services
{
    [TestFixture]
    public class SearchTableResultBuilderTest
    {

        [Test]
        public void GivenValidResponseShouldGenerateValidResultTable()
        {
            // Arrange
            var rowData = new List<string>
            {
                JsonConvert.SerializeObject (new
                {
                    Id = 10,
                    Email = "tory@sfa.com",
                    Name = "Tory Magnet",
                    Status = "Active"
                }),
                 JsonConvert.SerializeObject(new
                {
                    Id = 20,
                    Email = "rowdata@sfa.com",
                    Name = "Dave Jones",
                    Status = "Active"
                })
            };

            var searchMetaData = new List<SearchResultMetadata>()
               {
                new SearchResultMetadata
                {
                  SearchResultCategory = "USER",
                  ColumnDefinitions = new List<SearchColumnDefinition>
                  {
                      new SearchColumnDefinition
                      {
                         Name = "Id",
                         HideColumn = true
                      },
                      new SearchColumnDefinition
                      {
                          Name = "Name",
                          Link = new LinkDefinition
                          {
                              Format = "/resource/?key=user&id={0}",
                              MapColumnName = "ID"
                          }
                      },
                      new SearchColumnDefinition
                      {
                          Name = "Email",
                      },
                      new SearchColumnDefinition
                      {
                            Name = "Status"
                      }
                  }
                }
            };

            var searchResponse = new SearchResponse
            {
                Results = new Dictionary<string, List<string>>()
                {
                    {
                        "USER", rowData
                    }
                },
                LastPage = 1,
                Page = 1,
                SearchTerm = "Dave",
                SearchResultsMetadata = searchMetaData
            };

            var sut = new SearchTableResultBuilder();
            var table = sut.CreateTableResult(searchResponse);

            var actualColumnCount = searchMetaData.First().ColumnDefinitions.Count(c => !c.HideColumn);

            table.Should().NotBeNull();
            table.Columns.Should().NotBeNull();
            table.Columns.Should().HaveCount(actualColumnCount);

            table.Rows.Should().NotBeNull();
            table.Rows.Should().HaveCount(rowData.Count());

            table.Rows.First().CellValues.Should().NotBeNull();
            table.Rows.SelectMany(x => x.CellValues).Should().HaveCount(actualColumnCount * rowData.Count());
        }


    }
}
