using KTS.Web.Api.Interfaces;
using System;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using System.Configuration;
using Amazon;

namespace KTS.Web.Api.Providers.DynamoDb
{
    public class DynamoDbClient : IDatabaseClient
    {
        private static readonly string BooksTableName = ConfigurationManager.AppSettings["BooksTableName"];
        private static readonly int NextObjectIDRecordID = int.Parse(ConfigurationManager.AppSettings["IdDefinitionRecordId"]);
        private static readonly string NextObjectIDFieldName = ConfigurationManager.AppSettings["IdDefinitionFieldName"];

        private static Lazy<AmazonDynamoDBClient> client = 
            new Lazy<AmazonDynamoDBClient>(() => new AmazonDynamoDBClient(
                ConfigurationManager.AppSettings["DynamoDbAccessKeyId"],
                ConfigurationManager.AppSettings["DynamoDbSecretAccessKey"],
                RegionEndpoint.USEast1));
        private static AmazonDynamoDBClient Client
        {
            get
            {
                return client.Value;
            }
        }

        private static Lazy<Table> booksTable = new Lazy<Table>(() => Table.LoadTable(Client, BooksTableName));
        private static Table BooksTable
        {
            get
            {
                return booksTable.Value;
            }
        }

        private async Task<int> GetNextObjectId()
        {
            var nextObjectId = await BooksTable.GetItemAsync(NextObjectIDRecordID);
            if (nextObjectId != null)
            {
                var nextId = int.Parse(nextObjectId[NextObjectIDFieldName]);
                nextObjectId[NextObjectIDFieldName] = nextId + 1;
                await BooksTable.PutItemAsync(nextObjectId);
                return nextId;
            }
            return -1;
        }

        public async Task<JToken> GetBookAsync(int id)
        {
            var book = await BooksTable.GetItemAsync(id);
            if (book != null)
            {
                return JToken.Parse(book.ToJson());
            }
            return null;
        }

        public async Task<int> CreateOrUpdateBookAsync(JToken newBook)
        {
            var document = Document.FromJson(newBook.ToString(Newtonsoft.Json.Formatting.None));
            if (!document.GetAttributeNames().Contains("objectID"))
            {
                document.Add("objectID", await this.GetNextObjectId());
            }
            await BooksTable.PutItemAsync(document);
            return document["objectID"].AsInt();
        }

        public async Task<bool> DeleteBookAsync(int id)
        {
            var document = await BooksTable.DeleteItemAsync(id);
            return true;
        }
    }
}