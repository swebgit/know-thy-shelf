using KTS.Web.Interfaces;
using System;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using System.Configuration;
using Amazon;
using KTS.Web.Objects;
using KTS.Web.Enums;
using System.Linq;

namespace KTS.Web.Data.DynamoDb
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

        public async Task<DatabaseJObject> GetBookAsync(int id)
        {
            var query = BooksTable.Query(id, new QueryFilter(DatabaseJObject.TYPE_PROPERTY_NAME, QueryOperator.Equal, DatabaseObjectType.Book.ToString()));
            var books = await query.GetRemainingAsync();
            var book = books.FirstOrDefault();
            if (book != null)
            {
                return new DatabaseJObject(JObject.Parse(book.ToJson()));
            }
            return null;
        }

        public async Task<DatabaseJObject> CreateOrUpdateBookAsync(DatabaseJObject book)
        {
            if (!book.ObjectId.HasValue)
            {
                book.ObjectId = await this.GetNextObjectId();
            }

            var document = Document.FromJson(book.JObject.ToString(Newtonsoft.Json.Formatting.None));
            await BooksTable.PutItemAsync(document);
            return book;
        }

        public async Task<bool> DeleteBookAsync(int id)
        {
            var document = await BooksTable.DeleteItemAsync(id);
            return true;
        }
    }
}