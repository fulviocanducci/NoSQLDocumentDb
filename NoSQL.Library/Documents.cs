using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using System;
using System.Threading.Tasks;

namespace NoSQL.Library
{
    public class Documents : IDisposable
    {
        private const string EndpointUri = "https://localhost:8081/";
        private const string PrimaryKey = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";
        public const string DatabaseName = "Todo";
        public  DocumentClient Client { get; private set; }

        public Documents()
        {
            Client = new DocumentClient(new Uri(EndpointUri), PrimaryKey, new ConnectionPolicy { EnableEndpointDiscovery = false });
            //VerifyDatabaseIfExists();
        }

        private async Task VerifyDatabaseIfExists()
        {
            try
            {
                await Client.ReadDatabaseAsync(DatabaseName);
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    await Client.CreateDatabaseAsync(new Database { Id = DatabaseName });
                }
                else
                {
                    throw;
                }
            }
        }       

        public void Dispose()
        {
            if (Client != null)
            {
                Client.Dispose();
                Client = null;
            }            
        }
    }
}
