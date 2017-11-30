﻿using System.Collections.Generic;

namespace SFA.DAS.Support.Indexer.ApplicationServices.Services
{
    public interface IIndexProvider
    {
        void CreateIndex<T>(string name);

        void DeleteIndex(string name);

        void IndexDocuments<T>(string name, IEnumerable<T> documents) where T : class;

        bool IndexExists(string indexName);
    }
}