using System;
using System.Collections.Generic;
using System.Linq;
using ESFA.DAS.Support.Indexer.ApplicationServices.Services;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;

namespace ESFA.DAS.Support.Indexer.Infrastructure.AzureSearch
{
    public class AzureSearchProvider : IIndexProvider
    {
        private readonly ISearchServiceClient _client;

        public AzureSearchProvider(ISearchServiceClient client)
        {
            _client = client;
        }

        public void CreateIndex<T>(string name)
        {
            var definition = new Index
            {
                Name = name,
                Fields = FieldBuilder.BuildForType<T>()
            };

            _client.Indexes.Create(definition);
        }

        public void DeleteIndex(string name)
        {
            _client.Indexes.Delete(name);
        }

        public void IndexDocuments<T>(string name, IEnumerable<T> documents) where T : class
        {
            var indexClient = _client.Indexes.GetClient(name);
            var pageNum = 0;
            var start = DateTimeOffset.UtcNow;
            do
            {
                var count = 1000; // 1000 MAX per Rest call
                var page = documents.Skip(pageNum * count).Take(count).ToArray();
                if (!page.Any())
                    break;

                var batch = IndexBatch.MergeOrUpload(page);
                // TODO this won't delete documents
                try
                {
                    Console.WriteLine($"{indexClient.BaseUri}");

                    indexClient.Documents.Index(batch);
                    Console.WriteLine($"Indexed {batch.Actions.Count()} Documents");
                }
                catch (IndexBatchException e)
                {
                    // Sometimes when your Search service is under load, indexing will fail for some of the documents in
                    // the batch. Depending on your application, you can take compensating actions like delaying and
                    // retrying. For this simple demo, we just log the failed document keys and continue.
                    Console.WriteLine(
                        "Failed to index some of the documents: {0}",
                        string.Join(", ", e.IndexingResults.Where(r => !r.Succeeded).Select(r => r.Key)));
                }
                pageNum++;
            } while (true);
            var finish = DateTimeOffset.Now;

            Console.WriteLine(
                $"Indexed {documents.ToArray().Length} Documents in {new TimeSpan(finish.Ticks - start.Ticks).Seconds} secs.");
        }

        public bool IndexExists(string indexName)
        {
            var exists = _client.Indexes.Exists(indexName);


            return exists;
        }
    }
}