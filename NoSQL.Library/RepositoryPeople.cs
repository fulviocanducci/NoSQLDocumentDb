using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using System;
using System.Threading.Tasks;

namespace NoSQL.Library
{
    public class RepositoryPeople
    {
        protected DocumentClient client { get; private set; }
        private const string collectionName = "people";
        private Uri DocumentUri = UriFactory.CreateDocumentCollectionUri(Documents.DatabaseName, collectionName);       

        public RepositoryPeople(Documents doc)
        {
            client = doc.Client;
            //VerifyCollectionIfExists().Wait();            
        }

        public async Task<People> Insert(People document)
        {
            await client.CreateDocumentAsync(DocumentUri, document);
            return document;
        }

        public async Task<ResourceResponse<Document>> Update(People document)
        {
            return await client.ReplaceDocumentAsync(GetDocumentCreateUri(document.Id), document);            
        }

        public async Task<ResourceResponse<Document>> Delete(string id)
        {
            return await client.DeleteDocumentAsync(GetDocumentCreateUri(id));
        }

        public async Task<People> Get(string id)
        {
            Document doc = await client.ReadDocumentAsync(GetDocumentCreateUri(id));
            return (People)((dynamic)doc);
                
        }

        #region _private
        private Uri GetDocumentCreateUri(string id)
        {
            return UriFactory
                .CreateDocumentUri(Documents.DatabaseName, collectionName, id);
        }

        private async Task VerifyCollectionIfExists()
        {
            try
            {
                await client.ReadDocumentCollectionAsync(collectionName);
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    await client.CreateDocumentCollectionAsync(
                        UriFactory.CreateDatabaseUri(Documents.DatabaseName),
                        new DocumentCollection() { Id = collectionName });
                }
                else
                {
                    throw;
                }
            }
        }
        #endregion


    }
}
