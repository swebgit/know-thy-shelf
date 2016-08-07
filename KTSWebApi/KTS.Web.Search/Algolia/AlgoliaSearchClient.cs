using KTS.Web.Interfaces;
using System;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using Algolia.Search;
using System.Configuration;
using Newtonsoft.Json;
using KTS.Web.Objects;
using KTS.Web.Enums;

namespace KTS.Web.Search.Algolia
{
    public class AlgoliaSearchClient : ISearchClient
    {
        private static readonly string PrimaryIndexName = ConfigurationManager.AppSettings["AlgoliaPrimaryIndexName"];
        private static Lazy<AlgoliaClient> client = 
            new Lazy<AlgoliaClient>(() => new AlgoliaClient(ConfigurationManager.AppSettings["AlgoliaApplicationId"], ConfigurationManager.AppSettings["AlgoliaWriteApiKey"]));
                
        private static AlgoliaClient Client
        {
            get
            {
                return client.Value;
            }
        }

        private Lazy<Index> primaryIndex = new Lazy<Index>(() => Client.InitIndex(PrimaryIndexName));
        private Index PrimaryIndex
        {
            get
            {
                return this.primaryIndex.Value;
            }
        }

        public async Task<bool> CreateOrUpdateBookIndexAsync(DatabaseJObject book)
        {
            try
            {
                var indexObject = await this.PrimaryIndex.GetObjectAsync(book.ObjectId.ToString(), new string[] { DatabaseFields.OBJECT_ID });
                if (indexObject != null)
                {
                    await PrimaryIndex.SaveObjectAsync(book.JObject);
                }
            }
            catch (AlgoliaException ex)
            {
                if (ex.Message.Equals("ObjectID does not exist"))
                {
                    await PrimaryIndex.AddObjectAsync(book.JObject, objectId: book.ObjectId.ToString());
                }
                else
                {
                    throw;
                }
            }
            return true;
        }

        public async Task<bool> DeleteBookIndexAsync(int id)
        {
            await PrimaryIndex.DeleteObjectAsync(id.ToString());
            return true;
        }
        
        public async Task<JToken> GetBooksAsync(string searchString, int pageNumber, int pageSize)
        {
            var query = new Query(searchString);
            query.SetNbHitsPerPage(pageSize);
            query.SetPage(pageNumber);
            query.SetAttributesToRetrieve(new[] { "objectId", "title", "author", "author_first" });
            query.SetAttributesToHighlight(new string[] { });
            var queryResult = await this.PrimaryIndex.SearchAsync(query);
            JToken returnValue;
            return queryResult.TryGetValue("hits", out returnValue) ? returnValue : null;
        }
    }
}