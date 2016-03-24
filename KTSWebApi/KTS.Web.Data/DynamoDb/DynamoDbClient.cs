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
using Amazon.DynamoDBv2.Model;
using System.Collections.Generic;
using KTS.Web.Encryption;

namespace KTS.Web.Data.DynamoDb
{
    public class DynamoDbClient : IDatabaseClient
    {
        private static readonly string DatabaseTableName = ConfigurationManager.AppSettings["BooksTableName"];
        private static readonly string UsernameIndexName = ConfigurationManager.AppSettings["UsernameIndexName"];
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

        private static Lazy<Table> databaseTable = new Lazy<Table>(() => Table.LoadTable(Client, DatabaseTableName));
        private static Table DatabaseTable
        {
            get
            {
                return databaseTable.Value;
            }
        }

        private async Task<int> GetNextObjectId()
        {
            var nextObjectId = await DatabaseTable.GetItemAsync(NextObjectIDRecordID);
            if (nextObjectId != null)
            {
                var nextId = int.Parse(nextObjectId[NextObjectIDFieldName]);
                nextObjectId[NextObjectIDFieldName] = nextId + 1;
                await DatabaseTable.PutItemAsync(nextObjectId);
                return nextId;
            }
            return -1;
        }

        public async Task<DatabaseJObject> GetBookAsync(int id)
        {
            var query = DatabaseTable.Query(id, new QueryFilter(DatabaseFields.OBJECT_TYPE, QueryOperator.Equal, DatabaseObjectType.BOOK));
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
            await DatabaseTable.PutItemAsync(document);
            return book;
        }

        public async Task<bool> DeleteBookAsync(int id)
        {
            var document = await DatabaseTable.DeleteItemAsync(id);
            return true;
        }

        public async Task<Result<List<ActivityClaim>>> ValidateCredentialsAsync(Credentials credentials)
        {
            var result = new Result<List<ActivityClaim>>();
            var queryRequest = new QueryRequest
            {
                TableName = DatabaseTableName,
                IndexName = UsernameIndexName,
                KeyConditionExpression = "#username = :v_Username",
                ExpressionAttributeNames = new Dictionary<string, string>
                {
                    { "#username", DatabaseFields.USERNAME }
                },
                ExpressionAttributeValues = new Dictionary<string, AttributeValue> {
                    {":v_Username", new AttributeValue { S =  credentials.Username }}
                }
            };

            var response = await Client.QueryAsync(queryRequest);
            var user = response.Items.Where(u => u.Keys.Contains(DatabaseFields.OBJECT_TYPE) && u[DatabaseFields.OBJECT_TYPE].S.Equals(DatabaseObjectType.USER))
                                     .FirstOrDefault();
            if (user != null && HashHelper.ValidateHashedValue(user[DatabaseFields.PASSWORD].S, credentials.Password))
            {
                result.ResultCode = ResultCode.Ok;
                ActivityClaim claim;
                result.Data = user[DatabaseFields.USER_CLAIMS].SS.Select(c => Enum.TryParse(c, out claim) ? claim : ActivityClaim.None).Where(c => c != ActivityClaim.None).ToList();
            }
            return result;
        }
    }
}