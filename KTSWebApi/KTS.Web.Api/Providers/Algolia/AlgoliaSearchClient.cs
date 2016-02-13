using KTS.Web.Api.Interfaces;
using System;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using Algolia.Search;
using System.Configuration;
using Newtonsoft.Json;

namespace KTS.Web.Api.Providers.Algolia
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

        public async Task<bool> CreateOrUpdateBookIndexAsync(JObject book, int id)
        {
            try
            {
                var indexObject = await this.PrimaryIndex.GetObjectAsync(id.ToString(), new string[] { "objectID" });
                if (indexObject != null)
                {
                    await PrimaryIndex.SaveObjectAsync(book);
                }
            }
            catch (AlgoliaException ex)
            {
                if (ex.Message.Equals("ObjectID does not exist"))
                {
                    await PrimaryIndex.AddObjectAsync(book, objectId: id.ToString());
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
    }
}