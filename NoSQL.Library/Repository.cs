using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
//https://docs.microsoft.com/en-us/azure/documentdb/documentdb-get-started
namespace NoSQL.Library
{
    public abstract class Repository<T> : IRepository<T>
        where T : class, new()
    {

        protected Documents doc { get; private set; }
        protected DocumentClient client { get; private set; }
        protected string collectionName { get; private set; } = "";

        public Repository(Documents doc)
        {
            this.doc = doc;
            client = doc.Client;
        }
        public Repository(Documents doc, string collectionName)
        {
            this.doc = doc;
            this.collectionName = collectionName;
            client = doc.Client;
        }

        public async Task<T> InsertAsync(T document)
        {
            await client.CreateDocumentAsync(GetDocumentUri(),
                document);
            return document;
        }

        public async Task<ResourceResponse<Document>> UpdateAsync(T document, string id)
        {
            return await client.ReplaceDocumentAsync(GetDocumentCreateUri(id), document);
        }

        public async Task<ResourceResponse<Document>> DeleteAsync(string id)
        {
            return await client.DeleteDocumentAsync(GetDocumentCreateUri(id));
        }

        public async Task<T> FindAsync(string id)
        {
            Document doc = await client.ReadDocumentAsync(GetDocumentCreateUri(id));
            return (T)((dynamic)doc);
        }

        public async Task<IEnumerable<T>> AllAsync(Expression<Func<T, bool>> where)
        {
            IDocumentQuery<T> docQuery = GetOrderedQueryableAsync()
                .Where(where)
                .AsDocumentQuery();
            return await GetAllListAsync(docQuery);
        }

        public async Task<IEnumerable<T>> AllAsync<TKey>(Expression<Func<T, bool>> where, Expression<Func<T, TKey>> orderBy)
        {
            IDocumentQuery<T> docQuery = GetOrderedQueryableAsync()
                .Where(where)
                .OrderBy(orderBy)
                .AsDocumentQuery();
            return await GetAllListAsync(docQuery);
        }

        public async Task<IEnumerable<TDocument>> AllAsync<TKey, TDocument>(Expression<Func<T, bool>> where, Expression<Func<T, TKey>> orderBy, Expression<Func<T, TDocument>> select)
        {
            IDocumentQuery<TDocument> docQuery = GetOrderedQueryableAsync()
                .Where(where)
                .OrderBy(orderBy)
                .Select(select)
                .AsDocumentQuery();
            return await GetAllListAsync(docQuery);
        }
        public IOrderedQueryable Query()
        {
            return GetOrderedQueryableAsync();
        }

        #region _private
        private IOrderedQueryable<T> GetOrderedQueryableAsync()
        {
            return client
                 .CreateDocumentQuery<T>(GetDocumentUri(),
                 new FeedOptions { MaxItemCount = -1 });
        }
        private async Task<IEnumerable<TDocument>> GetAllListAsync<TDocument>(IDocumentQuery<TDocument> docQuery)
        {
            List<TDocument> _list = new List<TDocument>();
            while (docQuery.HasMoreResults)
            {
                _list.AddRange(await docQuery.ExecuteNextAsync<TDocument>());
            }
            return _list.Count == 0 ? null : _list;
        }
        private Uri GetDocumentCreateUri(string id)
        {
            return UriFactory
                .CreateDocumentUri(doc.DatabaseName, collectionName, id);
        }
        private Uri GetDocumentUri()
        {
            return UriFactory
                 .CreateDocumentCollectionUri(doc.DatabaseName, collectionName);
        }
        #endregion

    }
}
